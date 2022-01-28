using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysic : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private Vector2 characterSize;
    private Vector2 halfCharacterSize { get { return characterSize * .5f; } }

    [SerializeField] private Vector2 sensorSize;
    private Vector2 halfSensorSize { get { return sensorSize * .5f; } }

    [SerializeField] private float widthSkin;
    [SerializeField] private int sensorCount;
    [SerializeField] private float sensorBuffer;
    [SerializeField] private float minSensorLength;
    [SerializeField] private LayerMask physicInteract;

    [Header("Physics Info")]
    [ReadOnly] public Collisions collisions;
    [ReadOnly] public Vector2 velocity;
    [ReadOnly] public Vector2 influenceVelocity;
    [SerializeField, ReadOnly] private float gravity;
    [SerializeField, ReadOnly] private float gravityDownMultiplier;
    [SerializeField] private float maxGravity;

    private Vector2 velocityCalculation;
    private Vector2 movementPoint;
    private (Vector2 min, Vector2 max) movementClamp;
    private RaycastHit2D[] verticalCast;
    private RaycastHit2D[] horizontalCast;
    private RaycastHit2D hitSide;
    private Transform body;


    [Header("Debug Section")]
    public DrawDebug drawCharacterSize;
    public DrawDebug drawWidthSkin;
    public DrawDebug drawSensor;
    public DrawDebug drawVelocity;
    public Vector2 min, max;

    public virtual void SensorsLate()
    {
        collisions.isGroundedPrev = collisions.isGrounded;
        collisions.isTouchWallPrev = collisions.isTouchWall;
    }

    public void OnInit(Transform body)
    {
        this.body = body;

        horizontalCast = new RaycastHit2D[sensorCount];
        verticalCast = new RaycastHit2D[sensorCount];
    }

    public void OnPreUpdate()
    {
        PreSensor();
        PostSensor();
        ValiditySensorDetect();
    }

    public void OnUpdate()
    {
        ValidityCheckerMovement();
        PostSensor();
        ValiditySensorDetect();
        CoyoteSensor();
        ClampValue();
        Move();
        SensorsLate();
    }

    public void OnGravityUpdate(PhysicsState state)
    {
        switch (state)
        {
            case PhysicsState.OnNormal:
                UpdateGravityOnNormal();
                break;
            case PhysicsState.OnWater:
                UpdateGravityOnWater();
                break;
        }
    }

    public void ChangeGravityInfo(float gravity, float gravityDownMultiplier)
    {
        this.gravity = gravity;
        this.gravityDownMultiplier = gravityDownMultiplier;
    }

    private void PreSensor()
    {
        collisions.Reset();
        GroundSensor();
    }

    private void UpdateGravityOnWater()
    {
        velocity.y -= gravity * Time.deltaTime;
        velocity.y -= gravityDownMultiplier;
    }

    private void UpdateGravityOnNormal()
    {
        if (!collisions.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            if (velocity.y < 0) velocity.y -= gravityDownMultiplier;
            if (velocity.y < maxGravity) velocity.y = maxGravity;
        }
        else
        {
            if (!GroundChecker())
            {
                velocity.y -= gravity * Time.deltaTime;
                if (velocity.y < 0) velocity.y -= gravityDownMultiplier;
                if (velocity.y < maxGravity) velocity.y = maxGravity;
            }
            else
            {
                velocity.y = 0f;
            }
        }
    }

    private void PostSensor()
    {
        movementClamp.min = body.position.ToVec2() + (-halfCharacterSize) + (Vector2.one * -minSensorLength);
        movementClamp.max = body.position.ToVec2() + (halfCharacterSize) + (Vector2.one * minSensorLength);

        velocityCalculation = velocity * Time.deltaTime;

        HorizontalSensor();
        VerticalSensor();
    }

    private void ClampValue()
    {
        movementPoint = body.position.ToVec2() + velocityCalculation;

        movementPoint.ClampVector(movementClamp.min + halfCharacterSize, movementClamp.max - halfCharacterSize);

        min = movementClamp.min + (halfCharacterSize);
        max = movementClamp.max - halfCharacterSize;
    }

    private void Move()
    {
        if (movementPoint.IsNaN()) movementPoint = body.position;
        body.position = movementPoint;
        if (drawVelocity.active) Debug.DrawRay(body.position, velocityCalculation * 25f, drawVelocity.color);
    }

    #region PhysicSensor
    private void VerticalSensor()
    {
        float predictedMovementHorizontal = 0f;

        Vector2 predictedPosition = body.position + (body.right * predictedMovementHorizontal);

        Vector2 pointCast1, pointCast2, pointCast;
        float tempSensorLength = minSensorLength;
        float predictMovement = Mathf.Abs(velocityCalculation.y);
        Vector2 directionRay = body.up.ToVec2();
        Vector2 directionCast = body.right.ToVec2();

        if (predictMovement > 0f) tempSensorLength = minSensorLength * 2f;

        pointCast1 = (new Vector2(-halfCharacterSize.x, halfCharacterSize.y)) + (directionCast * sensorBuffer);
        pointCast2 = (halfCharacterSize) - (directionCast * sensorBuffer);

        for (int i = 0; i < sensorCount; i++)
        {
            pointCast = Vector2.Lerp(pointCast1, pointCast2, (float)i / (sensorCount - 1));
            verticalCast[i] = Physics2D.Raycast(predictedPosition + pointCast, directionRay, tempSensorLength, physicInteract);

            if (verticalCast[i])
            {
                if (drawSensor.active) Debug.DrawLine(predictedPosition + pointCast, verticalCast[i].point, drawSensor.color);
                movementClamp.max.y = Mathf.Min(movementClamp.max.y, verticalCast[i].point.y);
            }
        }

        pointCast1 = (-halfCharacterSize) + (directionCast * sensorBuffer);
        pointCast2 = (new Vector2(halfCharacterSize.x, -halfCharacterSize.y)) - (directionCast * sensorBuffer);

        for (int i = 0; i < sensorCount; i++)
        {
            pointCast = Vector2.Lerp(pointCast1, pointCast2, (float)i / (sensorCount - 1));
            verticalCast[i] = Physics2D.Raycast(predictedPosition + pointCast, -directionRay, tempSensorLength, physicInteract);

            if (verticalCast[i])
            {
                if (drawSensor.active) Debug.DrawLine(predictedPosition + pointCast, verticalCast[i].point, drawSensor.color);
                movementClamp.min.y = Mathf.Max(movementClamp.min.y, verticalCast[i].point.y);
            }
        }
    }

    private void HorizontalSensor()
    {
        Vector2 pointCast1, pointCast2, pointCast;
        float tempSensorLength = minSensorLength;
        float predictMovement = Mathf.Abs(velocity.x);
        if (predictMovement > tempSensorLength) tempSensorLength = predictMovement;
        Vector2 directionRay = body.right.ToVec2();
        Vector2 directionCast = body.up.ToVec2();

        pointCast1 = (new Vector2(halfCharacterSize.x, -halfCharacterSize.y)) + (directionCast * sensorBuffer);
        pointCast2 = (halfCharacterSize) - (directionCast * sensorBuffer);

        for (int i = 0; i < sensorCount; i++)
        {
            pointCast = Vector2.Lerp(pointCast1, pointCast2, (float)i / (sensorCount - 1));
            horizontalCast[i] = Physics2D.Raycast(body.position.ToVec2() + pointCast, directionRay, tempSensorLength, physicInteract);

            if (horizontalCast[i])
            {
                if (drawSensor.active) Debug.DrawLine(body.position.ToVec2() + pointCast, horizontalCast[i].point, drawSensor.color);
                movementClamp.max.x = Mathf.Min(movementClamp.max.x, horizontalCast[i].point.x);
            }
        }

        pointCast1 = (-halfCharacterSize) + (directionCast * sensorBuffer);
        pointCast2 = (new Vector2(-halfCharacterSize.x, halfCharacterSize.y)) - (directionCast * sensorBuffer);

        for (int i = 0; i < sensorCount; i++)
        {
            pointCast = Vector2.Lerp(pointCast1, pointCast2, (float)i / (sensorCount - 1));
            horizontalCast[i] = Physics2D.Raycast(body.position.ToVec2() + pointCast, -directionRay, tempSensorLength, physicInteract);

            if (horizontalCast[i])
            {
                if (drawSensor.active) Debug.DrawLine(body.position.ToVec2() + pointCast, horizontalCast[i].point, drawSensor.color);
                movementClamp.min.x = Mathf.Max(movementClamp.min.x, horizontalCast[i].point.x);
            }
        }
    }

    private void GroundSensor()
    {
        Vector2 pointCast1, pointCast2;
        float distance = widthSkin + sensorBuffer;
        Vector2 directionCast = body.up.ToVec2();

        pointCast1 = (-halfCharacterSize) + (directionCast * sensorBuffer);
        pointCast2 = (new Vector2(halfCharacterSize.x, -halfCharacterSize.y)) + (directionCast * sensorBuffer);

        RaycastHit2D leftCast = Physics2D.Raycast(body.position.ToVec2() + pointCast1, -directionCast, distance, physicInteract);
        RaycastHit2D rightCast = Physics2D.Raycast(body.position.ToVec2() + pointCast2, -directionCast, distance, physicInteract);

        if (leftCast || rightCast)
        {
            collisions.isGrounded = true;
        }
    }

    private bool GroundChecker()
    {
        Vector2 pointCast1, pointCast2;
        float distance = widthSkin + sensorBuffer;
        Vector2 directionCast = body.up.ToVec2();

        pointCast1 = (-halfCharacterSize) + (directionCast * sensorBuffer);
        pointCast2 = (new Vector2(halfCharacterSize.x, -halfCharacterSize.y)) + (directionCast * sensorBuffer);

        RaycastHit2D leftCast = Physics2D.Raycast(body.position.ToVec2() + pointCast1, -directionCast, distance, physicInteract);
        RaycastHit2D rightCast = Physics2D.Raycast(body.position.ToVec2() + pointCast2, -directionCast, distance, physicInteract);

        return (leftCast || rightCast);
    }

    private bool WallCheckerLeft()
    {
        Vector2 pointCast1, pointCast2;
        float distance = widthSkin + sensorBuffer;
        Vector2 directionCast = body.right.ToVec2();

        pointCast1 = (new Vector2(-halfCharacterSize.x, halfCharacterSize.y)) + (directionCast * sensorBuffer);
        pointCast2 = (new Vector2(-halfCharacterSize.x, -halfCharacterSize.y)) + (directionCast * sensorBuffer);

        RaycastHit2D topCast = Physics2D.Raycast(body.position.ToVec2() + pointCast1, -directionCast, distance, physicInteract);
        RaycastHit2D downCast = Physics2D.Raycast(body.position.ToVec2() + pointCast2, -directionCast, distance, physicInteract);

        return (topCast || downCast);
    }

    private bool WallCheckerRight()
    {
        Vector2 pointCast1, pointCast2;
        float distance = widthSkin + sensorBuffer;
        Vector2 directionCast = body.right.ToVec2();

        pointCast1 = (new Vector2(halfCharacterSize.x, halfCharacterSize.y)) - (directionCast * sensorBuffer);
        pointCast2 = (new Vector2(halfCharacterSize.x, -halfCharacterSize.y)) - (directionCast * sensorBuffer);

        RaycastHit2D topCast = Physics2D.Raycast(body.position.ToVec2() + pointCast1, directionCast, distance, physicInteract);
        RaycastHit2D downCast = Physics2D.Raycast(body.position.ToVec2() + pointCast2, directionCast, distance, physicInteract);

        return (topCast || downCast);
    }

    private void ValidityCheckerMovement()
    {
        if (WallCheckerLeft() && velocity.x < 0)
        {
            velocity.x = 0;
        }

        if (WallCheckerRight() && velocity.x > 0)
        {
            velocity.x = 0;
        }
    }

    private void ValiditySensorDetect()
    {
        if (body.position.y + halfCharacterSize.y >= movementClamp.max.y - halfCharacterSize.y)
        {
            collisions.isTouchTop = true;
            if (velocity.y > 0) velocity.y = 0;
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(body.position + (-body.right * (halfCharacterSize.x - widthSkin)), -body.up, halfCharacterSize.y + (2f * widthSkin), physicInteract);
        RaycastHit2D hitRight = Physics2D.Raycast(body.position + (body.right * (halfCharacterSize.x - widthSkin)), -body.up, halfCharacterSize.y + (2f * widthSkin), physicInteract);
        RaycastHit2D hitCenter = Physics2D.Raycast(body.position, -body.up, halfCharacterSize.y + (2f * widthSkin), physicInteract);
        
        Debug.DrawRay(body.position, -body.up * (halfCharacterSize.y + widthSkin), Color.red);
        if (hitCenter || hitLeft || hitRight)
        {
            collisions.isGrounded = true;
        }

        if (body.position.x <= movementClamp.min.x + halfCharacterSize.x + widthSkin)
        {
            hitSide = Physics2D.Raycast(body.position, -body.right, minSensorLength, physicInteract);
            if (hitSide)
            {
                collisions.isTouchLeft = true;
            }
        }

        if (body.position.x >= movementClamp.max.x - halfCharacterSize.x - widthSkin)
        {
            hitSide = Physics2D.Raycast(body.position, body.right, minSensorLength, physicInteract);
            if (hitSide)
            {
                collisions.isTouchRight = true;
            }
        }

        // touch wall
        collisions.isTouchWall = collisions.isTouchLeft || collisions.isTouchRight;
    }

    private void CoyoteSensor()
    {
        // coyote
        if (collisions.isGrounded)
        {
            if (!collisions.isGroundedPrev)
            {
                //velocity.y *= .5f;
                collisions.groundedTime = Time.time;
            }
            collisions.coyoteTime = 0;
        }
        else
        {
            collisions.coyoteTime += Time.deltaTime;
        }
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        // Draw Character Size
        DrawCharacterSize();
    }

    private void DrawCharacterSize()
    {
        if (drawCharacterSize.active)
        {
            if (body) GizmosHelper.DrawRect(body.position.ToVec2() - halfCharacterSize, body.position.ToVec2() + halfCharacterSize, drawCharacterSize.color);
            else GizmosHelper.DrawRect(transform.position.ToVec2() - halfCharacterSize, transform.position.ToVec2() + halfCharacterSize, drawCharacterSize.color);
        }

        if (drawWidthSkin.active)
        {
            if (body) GizmosHelper.DrawRect(body.position.ToVec2() - (halfCharacterSize - (Vector2.one * widthSkin / 2f)), body.position.ToVec2() + (halfCharacterSize - (Vector2.one * widthSkin / 2f)), drawWidthSkin.color);
            else GizmosHelper.DrawRect(transform.position.ToVec2() - (halfCharacterSize - (Vector2.one * widthSkin / 2f)), transform.position.ToVec2() + (halfCharacterSize - (Vector2.one * widthSkin / 2f)), drawWidthSkin.color);
        }

        if (drawSensor.active)
        {
            if (body) GizmosHelper.DrawRect(body.position.ToVec2() - halfSensorSize, body.position.ToVec2() + halfSensorSize, drawSensor.color);
            else GizmosHelper.DrawRect(transform.position.ToVec2() - halfSensorSize, transform.position.ToVec2() + halfSensorSize, drawSensor.color);
        }
    }

    [System.Serializable]
    public class DrawDebug
    {
        public bool active;
        public Color color;
    }
    #endregion

    #region Struct
    public enum PhysicsState { OnWater, OnNormal }

    [System.Serializable]
    public struct Collisions
    {
        public bool isGrounded;
        public bool isGroundedPrev;
        public bool isTouchLeft;
        public bool isTouchRight;
        public bool isTouchWall;
        public bool isTouchWallPrev;
        public bool isTouchTop;

        public float coyoteTime;
        public float jumpPressTime;
        public float groundedTime;

        public void Reset()
        {
            isGrounded = false;
            isTouchLeft = false;
            isTouchRight = false;
            isTouchWall = false;
            isTouchTop = false;
        }
    }
    #endregion
}
