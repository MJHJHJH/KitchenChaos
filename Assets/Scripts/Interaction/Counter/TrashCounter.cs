using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    protected override void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {
        if (e.baseCounter == this)
        {
            (sender as PlayerController).DropKichenObject();
        }
    }
}
