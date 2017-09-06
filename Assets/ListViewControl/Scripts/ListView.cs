using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
[RequireComponent(typeof(ScrollRect))]
public class ListView : MonoBehaviour {

	public RectTransform ContentRecTransform;
	public bool initialize = true;
	public float DistanceBetweenElements = 10;
	public bool AlwaysScroll = true;
	public RectTransform[] Content = null;
	public bool userefresh = false;
	public bool RotateRefreshImage = false;
	public float TopOffsetForRefreshing = 50;
	public Image refreshImage;
	public UnityEvent OnRefresh;

	public bool refreshLock = false;

	
	void Start () {
		if(initialize){
			InitializeList ();
		}

	}
		
	public void InitializeList(){
		ContentRecTransform.gameObject.SetActive (true);
		if(Content.Length == 0){
			List<RectTransform> children = new List<RectTransform> ();
			foreach(RectTransform rt in ContentRecTransform){
				if(rt.gameObject.activeSelf){
					children.Add (rt);
				}

			}
			Content  = children.ToArray ();
		}

		CreateList ();

		refreshLock = true;
	}
		
	void Update () {
		if(userefresh){
			if(!refreshLock && ContentRecTransform.offsetMax.y < 0){
				float percentage = ContentRecTransform.offsetMax.magnitude / TopOffsetForRefreshing;
				if (ContentRecTransform.offsetMax.magnitude < TopOffsetForRefreshing) {
					Color transparentColor = refreshImage.color;
					transparentColor.a = percentage;
					refreshImage.color = transparentColor;
					if(RotateRefreshImage){
						refreshImage.transform.rotation = new Quaternion(0,0,0,0);
						refreshImage.transform.Rotate (0,0,-360*percentage);
					}

				} else {
					Color transparentColor = refreshImage.color;
					transparentColor.a = 1.0f;
					refreshImage.color = transparentColor;
					refreshLock = true;
					OnRefresh.Invoke ();
				}
			}else{
				if(ContentRecTransform.offsetMax.magnitude==0){
					refreshLock = false;
				}
			}
		}
	}

	void CreateList(){
		float currentY = 0;
		foreach(RectTransform item in Content){
			TestForRectAnchorPositions (item);
			SetListItemRectPos (item,currentY);
			currentY += item.rect.height + DistanceBetweenElements;
		}
		SetContentRectHeight (currentY - DistanceBetweenElements);

	}

	void TestForRectAnchorPositions(RectTransform rt){
		if(rt.anchorMin.x != 0 || rt.anchorMin.y != 1 || rt.anchorMax.x!= 1 || rt.anchorMax.y!=1){
			throw new System.Exception ("All ListItems inside an ListView must have anchor position as Stretch-Top!");
		}
	}

	void SetListItemRectPos(RectTransform listItem, float posY){
		float contentHeight = ContentRecTransform.rect.height;
		Vector2 initialSize = listItem.sizeDelta;
		listItem.offsetMax = - new Vector2 (1,posY);
		listItem.offsetMin = - new Vector2 (0,(posY+initialSize.y));
		listItem.anchorMin = new Vector2 (0,1);
		listItem.anchorMax = new Vector2 (1, 1);

	}

	void SetContentRectHeight(float posY){

		ContentRecTransform.anchorMax = new Vector2(1,1);
		ContentRecTransform.anchorMin = new Vector2(0,1);
		ContentRecTransform.offsetMax = new Vector2(0,0);
		if(posY < (gameObject.transform as RectTransform).rect.height){
			ContentRecTransform.offsetMin = new Vector2(0,- (gameObject.transform as RectTransform).rect.height);
		}else{
			ContentRecTransform.offsetMin = new Vector2(0,- posY);
		}
		ScrollRect scrollRect = gameObject.GetComponent<ScrollRect> (); 
		if (ContentRecTransform.sizeDelta.y <= (scrollRect.transform as RectTransform).rect.height && !AlwaysScroll) {
			scrollRect.vertical = false;
		}
	}


}
	