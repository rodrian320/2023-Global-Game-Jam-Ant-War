using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Island : MonoBehaviour
{
    public HashSet<Island> linkIslands = new HashSet<Island>();

    [Header("Detail")]
    public int islandID;
    public int spawnRate;
    public int startNum;
    public int currentNum;
    public IslandState state;
    public int numUpdateInterval = 3;

    [Space]

    public GameObject antPrefeb;
    public Transform antParentTransform;
    public GameObject highlight;
    public int spawnInterval = 20;
    public float transportTime = 4.5f;


    [Space]
    public SpriteRenderer islandSpriteRenderer;
    public Text numText;
    public Text islandIDText;

    private float totalTime = 0;
    int remainSpawnCount;

   // int selectNum = 0;
    bool isSelected;
    bool isTransporting;

    List<GameObject> ants = new List<GameObject>();

    private void Start()
    {



        islandIDText.text = islandID.ToString();
        print(this.name+"" + islandID + islandIDText.text);
        numText.text = startNum.ToString();
        currentNum = startNum;
        
        for (int i = 0; i < currentNum / spawnInterval; i++)
        {
            GameObject obj = Instantiate(antPrefeb, antParentTransform);
            obj.transform.position = transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
            obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            ants.Add(obj);
        }

        remainSpawnCount = currentNum % spawnInterval;

        ChangeIslandState(state);

    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D mouseHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (mouseHit.collider.transform.position == this.transform.position && !isTransporting)
            {
                isSelected = !isSelected;
                highlight.SetActive(!highlight.activeSelf);
                //GameManager.instance.isSelected[islandID] = highlight.activeSelf;
            }   
        }


        if (isSelected && !isTransporting)
        {
            foreach (Island island in linkIslands)
            {
                if (island.isSelected)
                {
                    print(island.islandID);
                    StartCoroutine(Transport(island));
                    
                    return;
                }
            }


        }



        AutoUpdateNum();


    }


    IEnumerator Transport(Island island)
    {
        isTransporting = true;

        float percent = 0;
        float transportSpeed = 1 / transportTime;

        Vector3[] initialPos = new Vector3[ants.Count+1];

        int i = 0;

        foreach (var ant in ants)
        {
            ant.transform.position = transform.position;
            initialPos[i++] = ant.transform.position;
        }

        

        while(percent < 1)
        {
            percent += Time.deltaTime * transportSpeed;
            foreach (var ant in ants)
            {
                ant.transform.position = Vector3.Lerp(initialPos[i], GenerateMap.instance.islandTransforms[island.islandID].position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0), percent);
            }
            yield return null;
        }

        yield return new WaitForSeconds(1);


        isTransporting = false;
        island.isSelected = false;
        isSelected = false;
        highlight.SetActive(false);
        island.highlight.SetActive(false);
        // GameManager.instance.isSelected[islandID] = false;
        //  GameManager.instance.isSelected[island.islandID] = false;
    }




    public void AutoUpdateNum()
    {
        totalTime += Time.deltaTime; //累加每帧消耗时间
        if (currentNum > 0 && totalTime >= numUpdateInterval)
        {
            currentNum += spawnRate;
            remainSpawnCount += spawnRate;
            numText.text = currentNum.ToString();
            totalTime = 0;//清空累加的时间
            if (remainSpawnCount >= spawnInterval)
            {
                for (int i = 0; i < remainSpawnCount / spawnInterval; i++)
                {
                    GameObject obj = Instantiate(antPrefeb, antParentTransform);
                    obj.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    ants.Add(obj);
                }

                remainSpawnCount %= spawnInterval;
            }
        }
    }


    public void ChangeIslandState(IslandState state)
    {
        switch (state)
        {
            case IslandState.ourSide:
                islandSpriteRenderer.color = Color.green;
                break;
            case IslandState.enemySide:
                islandSpriteRenderer.color = Color.red;
                break;
            case IslandState.neutral:
                islandSpriteRenderer.color = Color.white;
                break;
        }
    }



}
