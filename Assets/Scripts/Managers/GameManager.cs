using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    public HashSet<Vector3Int> Blocked { get => blocked; set => blocked = value; }

    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();

    [SerializeField]
    private LayerMask clickableLayer, groundLayer;

    [SerializeField]
    private Player player;

    private Enemy currentTarget;

    private Camera mainCamera;

    private int targetIndex;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        ClickTarget();
        NextTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);
    
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                if (currentTarget != null)
                {
                    DeselectTarget();
                }

                selectTarget(hit.collider.GetComponent<Enemy>());
            }
            else //Deselect target // click ground
            {
                UIManager.Instance.HideTargetFrame();

                if (currentTarget != null)
                {
                    DeselectTarget();
                }
                currentTarget = null;
                player.Target = null;

                RaycastHit2D hitGround = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, groundLayer);

                if (hitGround.collider != null)
                {
                    player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
                }

            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickableLayer);

            if (hit.collider != null)
            {
                IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

                if (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable")
                {
                    if (player.Interactables.Contains(entity))
                    {
                        entity.Interact();
                    }
                }
            }
          
            
        }
    }

    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeselectTarget();

            if (Player.Instance.Attackers.Count > 0)
            {
                if(targetIndex < Player.Instance.Attackers.Count)
                {
                    selectTarget(Player.Instance.Attackers[targetIndex]);

                    targetIndex++;

                    if (targetIndex >= Player.Instance.Attackers.Count)
                    {
                        targetIndex = 0;
                    }
                }
                else
                {
                    targetIndex = 0;
                }
               

                    
            }

        }
    }


    private void DeselectTarget()
    {
        if(currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }

    private void selectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.Target = currentTarget.Select();
        UIManager.Instance.ShowTargetFrame(enemy);
    }

    public void OnKillConfirmed(Character character)
    {
        if(killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }


}
