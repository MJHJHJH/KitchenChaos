using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kichenObjectSO;
    [SerializeField] private Transform topAnchorPoint;
    private const int MaxPlateNum = 4;
    private List<PlatesObject> allPlatesKichenObject = new List<PlatesObject>(MaxPlateNum);

    private PlatesObject GetKichenObject()
    {
        if (allPlatesKichenObject.Count > 0)
        {
            PlatesObject currentKichenObject = allPlatesKichenObject[0];
            allPlatesKichenObject.RemoveAt(0);
            RefreshAllPlatesKichenObject();
            return currentKichenObject;
        }
        return null;
    }

    private float spawnTimer = 0f;
    private float spawnTimerMax = 2f;
    private void Update()
    {
        if (allPlatesKichenObject.Count < MaxPlateNum)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTimerMax)
            {
                spawnTimer = 0f;
                PlatesObject kichenObject = PlatesObject.CreateKichenObject(kichenObjectSO, this);
                allPlatesKichenObject.Add(kichenObject);
                RefreshAllPlatesKichenObject();
            }
        }
    }

    protected override void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {
        if (e.baseCounter == this)
        {
            PlayerController playerController = (sender as PlayerController);
            if (!playerController.IsHandKichenObject())
            {
                PlatesObject currentKichenObject = GetKichenObject();
                if (currentKichenObject != null)
                {
                    playerController.GetKichenObject(currentKichenObject);
                }
            }
        }
    }

    private void RefreshAllPlatesKichenObject()
    {
        for (int i = 0; i < allPlatesKichenObject.Count; i++)
        {
            allPlatesKichenObject[i].
                GetGameObject().transform.localPosition = new Vector3(0, 0.1f * i, 0);
        }
    }

    public void ClearKichenObject(KitchenObject clearKichenObject)
    {

    }

    public void GetNewKichenObject(KitchenObject newKichenObject)
    {

    }

    public Transform GetTopAnchorPoint() { return topAnchorPoint; }
}
