using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridWidth = 11;
    public int gridHeight = 11;

    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
    public float gap = 0.0f;

    Vector3 startPos;

    [SerializeField] protected List<int> randomNums;

    // 0-wood, 1-clay, 2-wheat, 3-stone, 4-sheep, 5-desert
    [SerializeField] protected List<Transform> resources;
    
    private void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        AddGap();
        CalcStartPos();
        StartCoroutine(CreateGrid());
    }

    private void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    private void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * 0.75f * (gridHeight / 2);

        startPos = new Vector3(x, 0, z);
    }

    private Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    private IEnumerator CreateGrid()
    {
        int multi = 1;
        int newY;

        int y = 0;
        int newX = gridWidth;
        int offset = 0;
        while(y <= gridHeight / 2)
        {
            newY = y * multi;
            for (int x = 0 + offset; x < newX + offset; x++)
            {
                Transform hex = RandomizeMap();
                Vector2 gridPos = new Vector2(x, newY);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                yield return new WaitForSeconds(0.5f);
            }

            if (multi == 1)
            {
                y++;
                newX--;

                if (y > 0 && y % 2 == 0)
                {
                    offset++;
                }
            }

            multi *= -1;
        }
    }

    private Transform RandomizeMap()
    {
        int randomNum;

        // resource index
        Transform randomPanel;

        // Instantiate resource panel
        randomPanel = resources[Random.Range(0, resources.Count)];

        Transform hex = Instantiate(randomPanel) as Transform;
        resources.Remove(randomPanel);

        // if the panel is not a desert
        if (randomPanel.GetComponent<ResourceInfo>().GetResourceID() != 5)
        {
            randomNum = randomNums[Random.Range(0, randomNums.Count)];
            randomNums.Remove(randomNum);

            hex.GetComponent<ResourceInfo>().SetRandom(randomNum);
        }

        return hex;
    }
}
