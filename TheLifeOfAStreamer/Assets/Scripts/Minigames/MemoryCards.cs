using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MemoryCards : MonoBehaviour, IPointerClickHandler
{
    private int myCardNumber = 0;
    private bool flippedState = false;
    private bool paused = false;

    public Sprite[] cards;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!paused && !flippedState) {
            flippedState = true;
            ChangeCard(myCardNumber);
            gameObject.GetComponentInParent<MemoryHandler>().ReportClicked(myCardNumber);
        }
    }

    public void AssignNumber(int assignNum) {
        myCardNumber = assignNum;
    }

    public bool IsFlipped() {
        return flippedState;
    }

    public void Reset() {
        flippedState = false;
        ChangeCard(0);
    }

    public void togglePause(bool flag) {
        paused = flag;
    }

    private void ChangeCard(int cardNum) {
        GetComponent<Image>().sprite = cards[cardNum];
    }
}
