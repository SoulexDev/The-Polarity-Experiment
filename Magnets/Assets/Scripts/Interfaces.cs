using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISource
{

}
public interface IInteractable
{
    public void Interact();
}
public interface IReceptical
{
    public void Activate(ISource source);
    public void Deactivate(ISource source);
    public void OverrideActivate();
}