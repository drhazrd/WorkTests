using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
	
	//Every interactable will share these
	//Create Data Folder
	
	float holdDuration { get; }
	
	bool holdInteract { get; }
	
	bool multipleUse { get; }
	
	bool isInteractable { get; }
	
	void OnInteract();
	
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class InputHandler: MonoBehaviour
{
	 #region Data
            [Space,Header("Input Data")]
            [SerializeField] private CameraInputData cameraInputData = null;
            [SerializeField] private MovementInputData movementInputData = null;
            [SerializeField] private InteractionInputData interactionInputData = null;
        #endregion

        #region BuiltIn Methods
            void Start()
            {
                cameraInputData.ResetInput();
                movementInputData.ResetInput();
                interactionInputData.ResetInput();
            }

            void Update()
            {
                GetCameraInput();
                GetMovementInputData();
                GetInteractionInputData();
            }
        #endregion

        #region Custom Methods
            void GetInteractionInputData()
            {
                interactionInputData.InteractedClicked = Input.GetKeyDown(KeyCode.E);
                interactionInputData.InteractedReleased = Input.GetKeyUp(KeyCode.E);
            }

            void GetCameraInput()
            {
                cameraInputData.InputVectorX = Input.GetAxis("Mouse X");
                cameraInputData.InputVectorY = Input.GetAxis("Mouse Y");

                cameraInputData.ZoomClicked = Input.GetMouseButtonDown(1);
                cameraInputData.ZoomReleased = Input.GetMouseButtonUp(1);
            }

            void GetMovementInputData()
            {
                movementInputData.InputVectorX = Input.GetAxisRaw("Horizontal");
                movementInputData.InputVectorY = Input.GetAxisRaw("Vertical");

                movementInputData.RunClicked = Input.GetKeyDown(KeyCode.LeftShift);
                movementInputData.RunReleased = Input.GetKeyUp(KeyCode.LeftShift);

                if(movementInputData.RunClicked)
                    movementInputData.IsRunning = true;

                if(movementInputData.RunReleased)
                    movementInputData.IsRunning = false;

                movementInputData.JumpClicked = Input.GetKeyDown(KeyCode.Space);
                movementInputData.CrouchClicked = Input.GetKeyDown(KeyCode.LeftControl);
            }
        #endregion
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
[CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData")]
public class InteractionInputData : ScriptableObject
{
	private bool m_interactedClicked;
	private bool m_interactedRelease;

	public bool InteractedClicked
	{
		get => m_interactedClicked;
		set => m_interactedClicked = value;
	}

	public bool InteractedReleased
	{
		get => m_interactedRelease;
		set => m_interactedRelease = value;
	}

	public void ResetInput()
	{
		m_interactedClicked = false;
		m_interactedRelease = false;
	}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class InteractableBase : MonoBehaviour, IInteractable
{
	#region Variables    
		[Space,Header("Interactable Settings")]

		[SerializeField] private bool holdInteract = true;
		[ShowIf("holdInteract")][SerializeField] private float holdDuration = 1f;
		
		[Space] 
		[SerializeField] private bool multipleUse = false;
		[SerializeField] private bool isInteractable = true;

		[SerializeField] private string tooltipMessage = "interact";
	#endregion

	#region Properties    
		public float HoldDuration => holdDuration; 

		public bool HoldInteract => holdInteract;
		public bool MultipleUse => multipleUse;
		public bool IsInteractable => isInteractable;

		public string TooltipMessage => tooltipMessage;
	#endregion

	#region Methods
	public virtual void OnInteract()
		{
			Debug.Log("INTERACTED: " + gameObject.name);
		}
	#endregion
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
[CreateAssetMenu(fileName = "Interaction Data", menuName = "InteractionSystem/InteractionData")]
public class InteractionData : ScriptableObject
{
	private InteractableBase m_interactable;

	public InteractableBase Interactable
	{
		get => m_interactable;
		set => m_interactable = value;
	}

	public void Interact()
	{
		m_interactable.OnInteract();
		ResetData();
	}

	public bool IsSameInteractable(InteractableBase _newInteractable) => m_interactable == _newInteractable;
	public bool IsEmpty() => m_interactable == null;
	public void ResetData() => m_interactable = null;

}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class InteractionController : MonoBehaviour
{
	#region Variables    
		[Space, Header("Data")]
		[SerializeField] private InteractionInputData interactionInputData = null;
		[SerializeField] private InteractionData interactionData = null;

		[Space, Header("UI")]
		[SerializeField] private InteractionUIPanel uiPanel;

		[Space, Header("Ray Settings")]
		[SerializeField] private float rayDistance = 0f;
		[SerializeField] private float raySphereRadius = 0f;
		[SerializeField] private LayerMask interactableLayer = ~0;


		#region Private
			private Camera m_cam;

			private bool m_interacting;
			private float m_holdTimer = 0f;
			
		#endregion

	#endregion

	#region Built In Methods      
		void Awake()
		{
			m_cam = FindObjectOfType<Camera>();
		}

		void Update()
		{
			CheckForInteractable();
			CheckForInteractableInput();
		}
	#endregion


	#region Custom methods         
		void CheckForInteractable()
		{
			Ray _ray = new Ray(m_cam.transform.position,m_cam.transform.forward);
			RaycastHit _hitInfo;

			bool _hitSomething = Physics.SphereCast(_ray,raySphereRadius, out _hitInfo, rayDistance, interactableLayer);

			if(_hitSomething)
			{
				InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();

				if(_interactable != null)
				{
					if(interactionData.IsEmpty())
					{
						interactionData.Interactable = _interactable;
						uiPanel.SetTooltip(_interactable.TooltipMessage);
					}
					else
					{
						if(!interactionData.IsSameInteractable(_interactable))
						{
							interactionData.Interactable = _interactable;
							uiPanel.SetTooltip(_interactable.TooltipMessage);
						}  
					}
				}
			}
			else
			{
				uiPanel.ResetUI();
				interactionData.ResetData();
			}

			Debug.DrawRay(_ray.origin,_ray.direction * rayDistance,_hitSomething ? Color.green : Color.red);
		}

		void CheckForInteractableInput()
		{
			if(interactionData.IsEmpty())
				return;

			if(interactionInputData.InteractedClicked)
			{
				m_interacting = true;
				m_holdTimer = 0f;
			}

			if(interactionInputData.InteractedReleased)
			{
				m_interacting = false;
				m_holdTimer = 0f;
				uiPanel.UpdateProgressBar(0f);
			}

			if(m_interacting)
			{
				if(!interactionData.Interactable.IsInteractable)
					return;

				if(interactionData.Interactable.HoldInteract)
				{
					m_holdTimer += Time.deltaTime;

					float heldPercent = m_holdTimer / interactionData.Interactable.HoldDuration;
					uiPanel.UpdateProgressBar(heldPercent);

					if(heldPercent > 1f)
					{
						interactionData.Interact();
						m_interacting = false;
					}
				}
				else
				{
					interactionData.Interact();
					m_interacting = false;
				}
			}
		}
	#endregion
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class DestroyInteractable : InteractableBase
{

	public override void OnInteract()
	{
		base.OnInteract();

		Destroy(gameObject);
	}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class InteractionUI : MonoBehaviour
{
	public Image holdProgressIMG;
	public Image tooltipBG;
	private RectTransform m_canvasTransform;
	private TextMeshProUGUI m_interactableTooltip;

	public void Init()
	{
		GetComponents();
	}

	void GetComponents()
	{
		m_canvasTransform = GetComponent<RectTransform>();
		m_interactableTooltip = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void SetToolTip(Transform _parent , string _tooltip, float _holdProgress)
	{
		if(_parent)
		{
			m_canvasTransform.position = _parent.position;
			m_canvasTransform.SetParent(_parent);
		}

		m_interactableTooltip.SetText(_tooltip);
		holdProgressIMG.fillAmount = _holdProgress;
	}

	public void SetTooltipActiveState(bool _state)
	{
		m_interactableTooltip.gameObject.SetActive(_state);
		holdProgressIMG.gameObject.SetActive(_state);
		tooltipBG.gameObject.SetActive(_state);
	}

	public void UpdateChargeProgress(float _progress)
	{
		holdProgressIMG.fillAmount = _progress;
	}

	public void LookAtPlayer(Transform _player)
	{
		m_canvasTransform.LookAt(_player,Vector3.up);
	}

	public void UnparentToltip()
	{
		m_canvasTransform.SetParent(null);
	}

	public bool IsTooltipActive()
	{
		return m_interactableTooltip.gameObject.activeSelf;
	}

}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class InteractionUIPanel : MonoBehaviour
{
	[SerializeField] private Image progressBar;
	[SerializeField] private TextMeshProUGUI tooltipText;

	public void SetTooltip(string tooltip)
	{
		tooltipText.SetText(tooltip);
	}

	public void UpdateProgressBar(float fillAmount)
	{
		progressBar.fillAmount = fillAmount;
	}

	public void ResetUI()
	{
		progressBar.fillAmount = 0f;
		tooltipText.SetText("");
	}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
[CreateAssetMenu(fileName = "PickableData", menuName = "FirstPersonController/Data/PickableData", order = 0)]
public class PickableData : ScriptableObject
{
	private Pickable pickable;
	public Pickable PickableItem
	{
		get => pickable;
		set => pickable = value;
	}

	public bool IsEmpty()
	{
		if (pickable != null)
			return false;
		else
			return true;
	}

	public bool IsSamePickable(Pickable _pickable)
	{
		if(pickable == _pickable)
			return true;
		else
			return false;
	}

	public void Pick()
	{
		pickable.OnPickUp();
	}

	public void Hold()
	{
		pickable.OnHold();
	}

	public void Release()
	{
		pickable.OnRelease();
	}

	public void ResetData()
	{
		pickable = null;
	}

}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
[CreateAssetMenu(fileName = "PickableInputData", menuName = "FirstPersonController/Data/PickableInputData", order = 0)]
public class PickableInputData : ScriptableObject
{
    private bool pickClicked;
    private bool pickHold;
    private bool pickReleased;

    public bool PickClicked
    {
        get => pickClicked;
        set => pickClicked = value;
    }
    public bool PickHold
    {
        get => pickHold;
        set => pickHold = value;
    }
    public bool PickReleased
    {
        get => pickReleased;
        set => pickReleased = value;
    }
}
