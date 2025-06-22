using System.Collections;
using UnityEngine;


public class EndlessLevelHandler : MonoBehaviour
{
    [SerializeField]
    GameObject[] sectionPrefabs;
    
    GameObject[] sectionsPoll = new GameObject[25];

    GameObject[] sections = new GameObject[10];

    [SerializeField] Transform playerCamTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);

    const float sectionLength = 10;



    void Start()
    {
        playerCamTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamTransform.transform.position = new Vector3(playerCamTransform.position.x,playerCamTransform.position.y,playerCamTransform.position.z + 2.0f);

        int prefabIndex = 0;
        for(int i=0;i<sectionsPoll.Length;i++){
            sectionsPoll[i] = Instantiate(sectionPrefabs[prefabIndex]);
            sectionsPoll[i].transform.eulerAngles = new Vector3(sectionsPoll[i].transform.position.x,sectionsPoll[i].transform.position.x + 180f,sectionsPoll[i].transform.position.z);
            sectionsPoll[i].SetActive(false);

            prefabIndex++;

            if(prefabIndex > sectionPrefabs.Length - 1)
                prefabIndex = 0;
        }

        for(int i=0;i<sections.Length;i++) 
        {
            GameObject randomSelection = GetRandomSectionFromPoll();

            randomSelection.transform.position = new Vector3(sectionsPoll[i].transform.position.x,0,i*sectionLength);
            randomSelection.SetActive(true);

            sections[i] = randomSelection;
        }
        StartCoroutine(UpdateLessOftenCO());
    }

    
    IEnumerator UpdateLessOftenCO() 
    {
        while(true){
            UpdateSectionsPositions();  
            yield return waitFor100ms;
        }
    }
    void UpdateSectionsPositions() {
        for(int i=0;i<sections.Length;i++) {
            if(sections[i].transform.position.z - playerCamTransform.position.z < -sectionLength) {
                Vector3 lastSectionPosition = sections[i].transform.position;
                sections[i].SetActive(false);

                sections[i] = GetRandomSectionFromPoll();

                sections[i].transform.position = new Vector3(lastSectionPosition.x,-100,lastSectionPosition.z + sectionLength * sections.Length);
                sections[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject GetRandomSectionFromPoll() 
    {
        int randomIndex = Random.Range(0,sectionsPoll.Length);

        bool isNewSectionFound = false;
        while(!isNewSectionFound) {
            if(!sectionsPoll[randomIndex].activeInHierarchy){
                isNewSectionFound = true;
            }
            else {
                randomIndex++;
                if(randomIndex > sectionsPoll.Length -1)
                    randomIndex = 0;
            }
        }
        return sectionsPoll[randomIndex];
    }
}
