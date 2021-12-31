using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;

public class BallsController : MonoBehaviour
{
    enum BallTpes
    {
        Dunk,
        Fifa
    }


    [SerializeField] float BodySpeed = 5;
    [SerializeField] float sideLerpSpeed;
    [SerializeField] int Gap = 10;

    [SerializeField] float speed;
    [SerializeField] float leftRightSpeed;
    [SerializeField] float rightLimit;
    [SerializeField] float leftLimit;

    [SerializeField] List<GameObject> BodyParts = new List<GameObject>();
    List<Vector3> PositionsHistory = new List<Vector3>();


    [SerializeField] Transform ballPrefab;
    [SerializeField] CinemachineVirtualCamera camMain;
    [SerializeField] Transform playerFifa;
    [SerializeField] Text ballsCountText;
     [SerializeField] Text ballsXpText;

    BallTpes bTypes = BallTpes.Dunk;

    [SerializeField] Transform head;


    void Start()
    {      
      
        ForwardJump(head.transform);
        camMain.m_Priority = 11;
        camMain.gameObject.SetActive(true);
        ballsCountText.text = "0";
        ballsXpText.text = "0";
    }

    Tween tweenForwardJump;
    void ForwardJump(Transform t)
    {
        if (IsFinish) return;
            var pos = t.localPosition;


        var jPower = (bTypes == BallTpes.Dunk) ? 1f : .5f;
        var tim = (bTypes == BallTpes.Dunk) ? .55f : .65f;

        if (bJump)
        {
            jPower = 3f;
            tim *= 2;
            bJump = false;
        }

        tweenForwardJump =  t.DOLocalJump(new Vector3(pos.x, 0.64f, pos.z+3f), jPower, 1, tim).SetEase(Ease.Linear).OnComplete(()=> {               
          
            ForwardJump(t);
        });
    }

    bool IsFinish = false;
    void Update()
    {
        ballsCountText.text = BodyParts.Count.ToString();

        if (IsFinish) return;

        PositionsHistory.Insert(0,head.position);
        MoveParts();
        Movement();

        //if (Input.GetKeyDown(KeyCode.Space)) AddNewBall();
        //if (Input.GetKeyDown(KeyCode.A)) camMain.m_Follow = BodyParts[1].transform;
    }

    void AddNewBall()
    {
        var t = Instantiate(ballPrefab, BodyParts[BodyParts.Count - 1].transform.position, Quaternion.identity);
        t.parent = transform;
        BodyParts.Add(t.gameObject);
        t.localScale = Vector3.zero;
        t.DOScale(Vector3.one, 1.75f).SetEase(Ease.OutQuint);       
        t.tag = "Destroyable";
        if (bTypes == BallTpes.Fifa)
        {
            t.GetChild(1).gameObject.SetActive(true);
            t.GetChild(2).gameObject.SetActive(false);

        }
    }

    private void MoveParts()
    {
        int index = 0 ;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];           
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;      
            index++;

        }       
    }

    private void LateUpdate()
    {
        if (IsFinish) return;
        BodyParts[0].transform.position = head.position;
    }


    #region Movement
    void Movement()
    {
        var currentPos = transform.position;

        if (Input.GetMouseButton(0))
        {
            var m = Mathf.Clamp(Input.GetAxis("Mouse X"), -.5f, .5f);
            var mouseInput = m * leftRightSpeed;

            Vector3 nPos = new Vector3(currentPos.x + mouseInput, currentPos.y, currentPos.z + speed);
            transform.position = Vector3.Lerp(transform.position, nPos, Time.deltaTime * .5f);

            CheckLimit(transform.position);
           

        }
        else
        {
            GoForward(currentPos);
          
           
        }
    }

   

    private void CheckLimit(Vector3 pos)
    {

        if (transform.position.x > rightLimit)
        {
            transform.position = new Vector3(rightLimit, pos.y, pos.z);
        }

        if (transform.position.x < leftLimit)
        {
            transform.position = new Vector3(leftLimit, pos.y, pos.z);
        }

    }
    void GoForward(Vector3 boatCurrentPos)
    {
       
        float speedChange = speed;
        var nPos = new Vector3(boatCurrentPos.x, boatCurrentPos.y, boatCurrentPos.z + speedChange);
        transform.position = Vector3.Lerp(transform.position, nPos, Time.deltaTime * .5f);
    }



    #endregion


    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.GetName().StartsWith("Cube"))
        {
           
            Collider myCollider = collision.contacts[0].thisCollider;
            myCollider.transform.GetChild(0).gameObject.SetActive(false);
            BodyParts.Remove(myCollider.gameObject);
            myCollider.transform.parent = null;
            myCollider.transform.DOScale(Vector3.zero, 3f).SetEase(Ease.OutQuint) ;
          
            var pos = myCollider.transform.position;
            myCollider.transform.DOJump(new Vector3(pos.x, 1f, pos.z - 5f), 2, 1, 1.5f);   


            collision.gameObject.SetActive(false);

        }
        if (collision.GetName().StartsWith("Finish"))
        {
            IsFinish = true;
            collision.gameObject.SetActive(false);
            StartCoroutine(FinishPoint());            
        }

        if (collision.GetName().StartsWith("BallPrefab"))
        {
            AddNewBall();
            collision.gameObject.SetActive(false);

        }
        if (collision.GetName().StartsWith("Gate"))
        {

            Collider myCollider = collision.contacts[0].thisCollider;
            myCollider.transform.GetChild(1).gameObject.SetActive(true);
            myCollider.transform.GetChild(2).gameObject.SetActive(false);

            bTypes = BallTpes.Fifa;
        }

        if (collision.GetName().StartsWith("basketBalJump"))
        {
            collision.collider.enabled = false;
            xp += 10 * BodyParts.Count;
            bJump = true;

            collision.transform.DOScale(new Vector3(.4f, collision.transform.localScale.y, .4f), .35f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);

            collision.transform.parent.GetChild(0).DOScale(new Vector3(1.3f, 1.3f, 1.3f), .1f).SetEase(Ease.OutQuint).
                SetLoops(BodyParts.Count, LoopType.Yoyo).SetDelay(.7f);


            ballsXpText.DOText(xp.ToString(), .1f * BodyParts.Count).SetDelay(.7f);
        }

        if (collision.GetName().StartsWith("Multy"))
        {
            collision.collider.enabled = false;

            for (int i = 0; i < 6; i++)
            {
                BodyParts[BodyParts.Count - 1].SetActive(false);
                BodyParts.RemoveAt(BodyParts.Count-1);
            }



        }



    }
    int xp = 0;

    bool bJump = false;

    IEnumerator FinishPoint()
    {
        int index = 0;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.DOMove(new Vector3(0.91f,0,0),.5f);

        foreach (var item in BodyParts)
        {
            var pos = item.transform.localPosition;
            item.transform.DOLocalJump(new Vector3(0, 0.64f + (index * .3f), BodyParts[0].transform.localPosition.z), .5f, 1, .5f);
            index++;

            yield return new WaitForSecondsRealtime(.05f);
        }
        yield return new WaitForSecondsRealtime(.5f);
        StartCoroutine(PlayFifa());

      

    }


    IEnumerator PlayFifa()
    {
        playerFifa.transform.GetChild(0).gameObject.SetActive(true);
        playerFifa.DOMoveZ(BodyParts[0].transform.position.z - 7, .1f);
        yield return new WaitForSecondsRealtime(.1f);

        StartCoroutine(PlayFifaControls());
        int index = 0;
        foreach (var item in BodyParts)
        {
            var pos = item.transform.localPosition;
            var pos2 = playerFifa.position + new Vector3(-.9f,0,-1) ;
            Tween tt = item.transform.DOLocalJump(pos2, .5f, 1, .6f);
            StartCoroutine(fifaBallMove(tt,item.transform));

            index++;

            for (int i = index; i < BodyParts.Count; i++)
            {
                var pos3 = BodyParts[i].transform.localPosition;
                BodyParts[i].transform.DOLocalJump(new Vector3(pos3.x,pos3.y-.3f,pos3.z), .1f, 1, .15f);
            }          

            yield return new WaitForSecondsRealtime(1f);
        }

        GameManager.instance.OnLevelClear();
    }


    IEnumerator fifaBallMove(Tween t,Transform ball)
    {
        bool isMoving = true;
        var p = ball.transform.localPosition;
        t.OnComplete(()=> { isMoving = false;
            ball.DOLocalJump(new Vector3(p.x, p.y + 2, p.z - 15), 0, 1, 1);

        });
      
        while (isMoving)
        {
            if (isKick)
            {
                var d = (ball.position.z - playerFifa.position.z);
                if (d < 1.5f && d > 0)
                {
                    print(d);
                    t.Kill();
                    ball.DOLocalJump(new Vector3(Random.Range(p.x-20,p.x-15),Random.Range(1,1),p.z+50),Random.Range(4,8),1,Random.Range(3,5));
                    isMoving = false;
                    xp += Random.Range(10,100);
                    ballsXpText.DOText(xp.ToString(), .5f);
                }             

                isMoving = false;
            }

            yield return new WaitForSeconds(.01f);
        }
      

    }

    bool isPlay = true;
    bool isKick = false;
    IEnumerator PlayFifaControls()
    {
        yield return new WaitForSecondsRealtime(1f);        
        Animator a = playerFifa.transform.GetChild(1).GetComponent<Animator>();
      
        while (isPlay)
        {
          
            if (Input.GetMouseButtonDown(0))
            {

                isKick = true;
                a.SetBool("IsKick",true);
                yield return new WaitForSecondsRealtime(.6f);
                a.SetBool("IsKick", false);
                isKick = false;

            }

            yield return new WaitForSeconds(.01f);
           
        }

    }


}
