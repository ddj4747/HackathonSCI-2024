using Unity.VisualScripting;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField]
    private GameObject upDoor;
    
    [SerializeField]
    private GameObject downDoor;

    private Vector3 upDoorStartPos;

    private Vector3 downDoorStartPos;

    public bool open = false;

    [SerializeField]
    float speed;

    [SerializeField]
    float diff;

    private void Start()
    {
        upDoorStartPos = upDoor.transform.position;
        downDoorStartPos = downDoor.transform.position;

        SoundManager.PlayMusic(Music.Hackathon1);
    }

    private void Update()
    {
        if (open)
        {
            if (upDoor.transform.position.y < upDoorStartPos.y + diff)
            {
                upDoor.transform.position += speed * Time.deltaTime * Vector3.up;
            }
            else
            {
                upDoor.transform.position = new Vector3(upDoorStartPos.x, upDoorStartPos.y + diff);
            }

            if (downDoor.transform.position.y > downDoorStartPos.y - diff)
            {
                downDoor.transform.position -= speed * Time.deltaTime * Vector3.up;
            }
            else
            {
                downDoor.transform.position = new Vector3(downDoorStartPos.x, downDoorStartPos.y - diff);
            }
        }
        else
        {
            if (upDoor.transform.position.y > upDoorStartPos.y)
            {
                upDoor.transform.position -= speed * Time.deltaTime * Vector3.up;
            }
            else
            {
                upDoor.transform.position = upDoorStartPos;
            }

            if (downDoor.transform.position.y < downDoorStartPos.y)
            {
                downDoor.transform.position += speed * Time.deltaTime * Vector3.up;
            }
            else
            {
                downDoor.transform.position = downDoorStartPos;
            }
        }
    }

}
