using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GunProjectile : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bullet;

    [Header("Bullet forces")]
    public float bulletSpeed;
    public float bulletUpwardForce;

    [Header("Gun settings")]

    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magSize;
    public int bulletsAClick;
    public bool allowSwitch;

    [Header("Graphics/UI")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammuunitionDisplay;

    //local variables
    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;

    [Header("Camera Reference")]
    public Camera camRef;

    [Header("Camera Reference")]
    public Transform attackPoint;

    [SerializeField]
    private bool allowInvoke;

    public void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        POEInput();

        if(ammuunitionDisplay != null)
        {
            ammuunitionDisplay.SetText("Ammo "+bulletsLeft / bulletsAClick + " / " + magSize / bulletsAClick);
        }
    }
    
    private void POEInput()
    {
        if (allowSwitch)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        if (Input.GetKeyDown(KeyCode.R)&&bulletsLeft < magSize && !reloading)
        {
            Reload();
        }
        //trying to shoot with no bullets relaods
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }
        
        if(readyToShoot&&shooting&&!reloading&&bulletsLeft > 0)
        {
            bulletsShot = 0;
            POEShoot();
        }

    }
    private void POEShoot()
    {
        readyToShoot = false;

        Ray ray = camRef.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPointer;
        if(Physics.Raycast(ray, out hit))
        {
            targetPointer = hit.point;
        }
        else
        {
            targetPointer = ray.GetPoint(100);
        }
        
        Vector3 directionWithNoSpread = targetPointer - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithNoSpread + new Vector3(x, y, 0);

        GameObject POEBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        POEBullet.transform.forward = directionWithSpread.normalized;

        POEBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletSpeed, ForceMode.Impulse);
        POEBullet.GetComponent<Rigidbody>().AddForce(camRef.transform.up * bulletUpwardForce, ForceMode.Impulse);
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
        //incase i end up making a shotgun
        if(bulletsShot < bulletsAClick && bulletsLeft > 0)
        {
            Invoke("POEShoot", timeBetweenShots);
        }
        
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
    
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadingFinished", reloadTime);
    }
    private void ReloadingFinshed()
    {
        bulletsLeft = magSize;
        reloading = false;
    }
}
