using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : ICommand
{
    private Action executeAction;
    private Action undoAction;
    public Command(Action executeAction,Action undoAction) {
        this.executeAction = executeAction;
        this.undoAction = undoAction;
    }
    public void Execute() {
        executeAction?.Invoke();
    }
    public void Undo() {
        undoAction?.Invoke();
    }

    

}
