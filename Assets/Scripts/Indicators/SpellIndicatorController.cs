using System.Collections;
using UnityEngine;

public class SpellIndicatorController : MonoBehaviour
{
    public static int SquareIndicator = 0;
    public static int ConeIndicator = 1;
    public static int CircleIndicator = 2;

    public float indicatorDeleteTimer = 10f;

    private GameObject indicator;
    private Material rangeCircleMaterial;
    private Material aoeCircleMaterial;
    private Material aoeSquareMaterial;
    private Material aoeConeMaterial;
    private Material defaultMaterial;

    public int mode { private get; set; }
    public bool picking { private get; set; }
    private int face;

    private Vector3 centerOfRadius;
    private float castingRadius;
    private Vector3 centerOfAOE;
    private Vector3 spellRotation;
    private float aoeRadius;
    private float aoeLength;
    private float aoeWidth;
    private float aoeHeight;

    private GameObject tmpRangeIndicator;
    private GameObject tmpAoeIndicator;
    private int layerMasks;
    //private Plane plane;

    private PlayerMovementScript controls;
    private bool mouse_1_clicked;
    private bool mouse_1_locked;

    private Transform startLocation;
    private Transform rayDirection;
    private float maxDistance;
    private Material lineMaterial;

    /*------------     Modes    ------------

    mode : 0  -  1 cirlce range and 1 circle aoe
    mode : 1  -  1 cirlce range and 1 square aoe
    mode : 2  -  locked range and 1 square aoe
    mode : 3  -  1 circle range and 1 square aoe with face swapping     ===== NOT USED =====
            face : 0  -  short and wide
            face : 1  -  tall and narrow
    mode : 4  -  spawns a lineRender to show hit position

    --------------------------------------*/

    // Start is called before the first frame update
    private void Awake()
    {
        mode = -1;
        mouse_1_clicked = false;
        mouse_1_locked = false;
        picking = false;
        controls = GameObject.FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript;
        layerMasks = LayerMask.GetMask("Ground");

        indicator = ResourceManager.Components.IndicatorBase;
        rangeCircleMaterial = ResourceManager.Materials.IndicatorCirlceRange;
        aoeCircleMaterial = ResourceManager.Materials.IndicatorCircleAOE;
        aoeSquareMaterial = ResourceManager.Materials.IndicatorSquareAOE;
        aoeConeMaterial = ResourceManager.Materials.IndicatorTriangleAOE;
        defaultMaterial = ResourceManager.Materials.DefaultMaterial;
    }

    private void Update()
    {
        if (controls != null)
            mouse_1_clicked = controls.mouse_1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (picking)
        {
            // Swap faces
            if (mode == 3 && mouse_1_clicked && !mouse_1_locked)
            {
                mouse_1_clicked = false;
                StartCoroutine(LockMouse_1(0.2f));
                SwapFaces();
            }

            // Center on player
            if (mode != 2)
            {
                if (controls != null)
                    centerOfRadius = controls.transform.position;
                tmpRangeIndicator.transform.position = centerOfRadius;
            }

            // Center on mouse
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMasks))
            {
                SetIndicators(hit, centerOfRadius, castingRadius);
            }
            else if (mode == 2)
            {
                SetIndicators(new RaycastHit(), centerOfRadius, 0f);
            }
        }
        else if (mode == 4)
        {
            Ray ray = new Ray(startLocation.position, rayDirection.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                SetLine(Color.red, startLocation.position, hit.point);
            }
            else
            {
                SetLine(Color.white, startLocation.position, ray.GetPoint(maxDistance));
            }
        }
    }

    private void SetIndicators(RaycastHit hit, Vector3 center, float range)
    {
        switch (mode)
        {
            // 1 circle and 1 polygon
            case 1:
                tmpAoeIndicator.transform.position = OutOfRange(hit.point, center, range);
                RotateToLookAtPlayer();
                break;
            
            // Locked center and 1 polygon
            case 2:
                RotateToLookAtPlayer();
                break;
                
            // 1 circle and 1 polygon, allow side swapping
            case 3:
                tmpAoeIndicator.transform.position = OutOfRange(hit.point, center, range);
                //Look at player

                if (Mathf.Abs(hit.normal.y) < 0.5)
                {
                    //Rotate to look away from walls
                    tmpAoeIndicator.transform.LookAt(tmpAoeIndicator.transform.position - hit.normal);
                }
                else
                {
                    //Rotate to look up
                    //Look at player
                    if (controls != null)
                        tmpAoeIndicator.transform.LookAt(controls.transform);

                    tmpAoeIndicator.transform.eulerAngles = new Vector3(
                        90f,
                        tmpAoeIndicator.transform.eulerAngles.y,
                        tmpAoeIndicator.transform.eulerAngles.z
                    );
                }
                break;

            // 2 circles
            default:
                tmpAoeIndicator.transform.position = OutOfRange(hit.point, center, range);
                break;
        }
        if (mode != 2)
        {
            centerOfAOE = tmpAoeIndicator.transform.position;
            tmpAoeIndicator.transform.position -= tmpAoeIndicator.transform.forward;
        }
    }

    private void RotateToLookAtPlayer()
    {
        if (tmpAoeIndicator == null)
            return;

        //Look at player
        if (controls != null)
            tmpAoeIndicator.transform.LookAt(controls.transform, Vector3.up);

        //Rotate to look up
        tmpAoeIndicator.transform.eulerAngles = new Vector3(
            90f,
            tmpAoeIndicator.transform.eulerAngles.y,
            tmpAoeIndicator.transform.eulerAngles.z
        );
    }

    private Vector3 OutOfRange(Vector3 hit, Vector3 center, float range)
    {
        if ((hit - center).magnitude <= range)
        {
            return hit;
        }
        else
        {
            return (hit - center).normalized * range + center;
        }
    }

    public void SelectLocation(float rangeRadious, float aoeSize)
    {
        // Use 2 circles
        mode = 0;

        // Reset previous indicators
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        // New indicator sizes
        castingRadius = rangeRadious;
        aoeRadius = aoeSize;
        // New indicators
        tmpRangeIndicator = Instantiate(indicator, centerOfRadius, indicator.transform.rotation);
        tmpRangeIndicator.GetComponent<MeshRenderer>().material = rangeCircleMaterial;
        tmpRangeIndicator.transform.localScale = Vector3.one * castingRadius * 2f;
        tmpAoeIndicator = Instantiate(indicator);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.AddComponent<KillOnDelayScript>();
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeCircleMaterial;
        tmpAoeIndicator.transform.localScale = Vector3.one * aoeRadius * 2f;
        tmpAoeIndicator.SetActive(true);

        picking = true;
    }
    public void SelectLocation(float rangeRadious, float leftToRight, float backToForward)
    {
        // Use 1 cirlce and 1 polygon
        mode = 1;

        // Reset previous indicators
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        // New indicator sizes
        castingRadius = rangeRadious;
        // New indicators
        tmpRangeIndicator = Instantiate(indicator, centerOfRadius, indicator.transform.rotation);
        tmpRangeIndicator.GetComponent<MeshRenderer>().material = rangeCircleMaterial;
        tmpRangeIndicator.transform.localScale = Vector3.one * castingRadius * 2f;
        tmpAoeIndicator = Instantiate(indicator);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.AddComponent<KillOnDelayScript>();
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeSquareMaterial;
        tmpAoeIndicator.transform.localScale = Vector3.right * leftToRight + Vector3.up * backToForward;
        tmpAoeIndicator.SetActive(true);

        picking = true;
    }

    public void SelectLocation(Transform center, float leftToRight, float backToForward, int aoeShape)
    {
        // Use locked center and 1 polygon
        mode = 2;

        // Reset previous indicators
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        // New indicators
        tmpAoeIndicator = Instantiate(indicator, center);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.AddComponent<KillOnDelayScript>();
        if (aoeShape == ConeIndicator)
        {
            tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeConeMaterial;
            tmpAoeIndicator.transform.Rotate(0f, 0f, 180f);
            tmpAoeIndicator.transform.position += center.forward * backToForward / 2f + Vector3.down * 2f;
        }
        else if (aoeShape == CircleIndicator)
        {
            tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeCircleMaterial;
            tmpAoeIndicator.transform.position += Vector3.down * 2f;
        }
        else
        {
            tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeSquareMaterial;
            tmpAoeIndicator.transform.position += center.forward * backToForward / 2f + Vector3.down * 2f;
        }
        tmpAoeIndicator.transform.localScale = Vector3.right * leftToRight + Vector3.up * backToForward;
        tmpAoeIndicator.SetActive(true);

        picking = true;
    }
    public void SelectLocation(float rangeRadious, Vector3 scaleVector)
    {
        // Use 1 cirlce and 1 polygon, allow for side swapping
        mode = 3;

        // Reset previous indicators
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        // New indicator sizes
        castingRadius = rangeRadious;
        aoeLength = scaleVector.x;
        aoeWidth = scaleVector.z;
        aoeHeight = scaleVector.y;
        // New indicators
        tmpRangeIndicator = Instantiate(indicator, centerOfRadius, indicator.transform.rotation);
        tmpRangeIndicator.GetComponent<MeshRenderer>().material = rangeCircleMaterial;
        tmpRangeIndicator.transform.localScale = Vector3.one * castingRadius * 2f;
        tmpAoeIndicator = Instantiate(indicator);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.AddComponent<KillOnDelayScript>();
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = aoeSquareMaterial;
        tmpAoeIndicator.transform.localScale = Vector3.right * aoeLength + Vector3.up * aoeWidth;
        tmpAoeIndicator.SetActive(true);

        face = 0;
        picking = true;
    }

    public void DisplayTargeting(Transform startLocation, Transform rayDirection, float width, float maxDistance)
    {
        // Show a line
        mode = 4;

        // Reset previous indicators
        if (tmpRangeIndicator != null) Destroy(tmpRangeIndicator);
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        // New indicators
        tmpAoeIndicator = Instantiate(indicator, rayDirection);
        tmpAoeIndicator.SetActive(false);
        tmpAoeIndicator.AddComponent<KillOnDelayScript>();
        tmpAoeIndicator.GetComponent<MeshRenderer>().material = defaultMaterial;
        tmpAoeIndicator.transform.localScale = Vector3.right * width;
        tmpAoeIndicator.SetActive(true);

        lineMaterial = tmpAoeIndicator.GetComponent<MeshRenderer>().material;

        this.startLocation = startLocation;
        this.rayDirection = rayDirection;
        this.maxDistance = maxDistance;
    }

    private void SetLine(Color color, Vector3 start, Vector3 end)
    {
        lineMaterial.SetColor("_Color", color);

        lineMaterial.SetFloat("_Alpha", 1.2f - (end - start).magnitude / maxDistance);

        tmpAoeIndicator.transform.position = start + rayDirection.forward * (end - start).magnitude / 2f;
        tmpAoeIndicator.transform.localScale = Vector3.right * tmpAoeIndicator.transform.localScale.x + Vector3.up * (end - start).magnitude;
    }

    public IndicatorResponse LockLocation()
    {
        picking = false;
        if (tmpAoeIndicator != null)
        {
            spellRotation = tmpAoeIndicator.transform.eulerAngles;
            if (mode != 3)
            {
                spellRotation = new Vector3(
                    0f,
                    spellRotation.y,
                    spellRotation.z
                );
            }
            else
            {
                spellRotation = new Vector3(
                       spellRotation.x + 90f,
                       spellRotation.y,
                       spellRotation.z
                   );
            }
            Destroy(tmpRangeIndicator);
        }
        else
        {
            return new IndicatorResponse().IsNull(true);
        }
        switch (mode)
        {
            case 1:
                return new IndicatorResponse().CenterOfAoe(centerOfAOE).SpellRotation(spellRotation);

            case 3:
                return new IndicatorResponse().CenterOfAoe(centerOfAOE).SpellRotation(spellRotation).Face(face);

            default:
                return new IndicatorResponse().CenterOfAoe(centerOfAOE);
        }
    }

    public void DestroyIndicator(float delay)
    {
        if (tmpAoeIndicator == null)
            return;

        KillOnDelayScript killScript = tmpAoeIndicator.GetComponent<KillOnDelayScript>();
        killScript.KillAfter(delay, this);
    }

    public void DestroyIndicator()
    {
        if (tmpAoeIndicator != null) Destroy(tmpAoeIndicator);
        mode = -1;
        picking = false;
    }

    private IEnumerator DestroyIndicatorDelay(float delay, GameObject indicator)
    {
        yield return new WaitForSeconds(delay);
        mode = -1;
        picking = false;
    }

    public void SwapFaces()
    {
        if (mode == 3)
        {
            switch (face)
            {
                case 1:
                    tmpAoeIndicator.transform.localScale = Vector3.right * aoeLength + Vector3.up * aoeWidth;
                    face = 0;
                    break;
                default:
                    tmpAoeIndicator.transform.localScale = Vector3.right * aoeHeight + Vector3.up * aoeWidth;
                    face = 1;
                    break;
            }
        }
    }

    IEnumerator LockMouse_1(float seconds)
    {
        mouse_1_locked = true;
        yield return new WaitForSeconds(seconds);
        mouse_1_locked = false;
    }
}
