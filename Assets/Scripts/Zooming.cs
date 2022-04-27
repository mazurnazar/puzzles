using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zooming : MonoBehaviour
{
	private Camera mainCamera;

	private float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

	private Vector2 firstTouchPrevPos, secondTouchPrevPos;

	[SerializeField]
	private float zoomModifierSpeed = 0.01f;

	private Vector3 touchStart;

	[SerializeField]
	private SpriteRenderer backgroundRenderer;
	
	private float mapMinX, mapMaxX, mapMinY, mapMaxY;
	private GameManager gameManager;

	private void Awake()
	{

		mainCamera = GetComponent<Camera>();
		
		mapMinX = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;
		mapMaxX = mainCamera.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect ;
		mapMinY = mainCamera.transform.position.y - mainCamera.orthographicSize ;
		mapMaxY = mainCamera.transform.position.y + mainCamera.orthographicSize ;
	}
	// Use this for initialization
	void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		
	}

	// Update is called once per frame
	void Update()
	{
		if (gameManager.IsGameRunning && !gameManager.PieceSelected)
		{
		    PanCamera();
			Zoom();
		}
	}
	private void Zoom()
    {
		if (Input.touchCount == 2)
		{
			Touch firstTouch = Input.GetTouch(0);
			Touch secondTouch = Input.GetTouch(1);

			firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
			secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

			touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
			touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

			zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

			if (touchesPrevPosDifference > touchesCurPosDifference)
				mainCamera.orthographicSize += zoomModifier;
			if (touchesPrevPosDifference < touchesCurPosDifference)
				mainCamera.orthographicSize -= zoomModifier;

		}

		mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 5f, 11f);
		mainCamera.transform.position = ClampCamera(mainCamera.transform.position);
	}
	private void PanCamera()
    {
		// save position of mouse in world space while drag starts (first time clicked)
		if (Input.GetMouseButtonDown(0))
		{
			touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
		// calculate difference between start and new position while draging
		if (Input.GetMouseButton(0))
		{
			Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);

			// move camera by that difference

			mainCamera.transform.position =  ClampCamera(mainCamera.transform.position + direction);
		}
	}

	private Vector3 ClampCamera(Vector3 targetPosition)
    {
		float camHeight = mainCamera.orthographicSize;
		float camWidth = mainCamera.orthographicSize * mainCamera.aspect;
		
		float minX = mapMinX + camWidth;
		float maxX = mapMaxX - camWidth;
		float minY = mapMinY + camHeight;
		float maxY = mapMaxY - camHeight;

		float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
		float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

		return new Vector3(newX, newY, targetPosition.z);
    }

}
