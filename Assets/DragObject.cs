using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector2 mousePosition;
    private float offsetX, offsetY;
    private static bool mouseButtonReleased; // Renamed variable for clarity

    private void OnMouseDown()
    {
        mouseButtonReleased = false;
        // Calculate the offset between the mouse position and the object's position
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x -transform.position.x;
        offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y -  transform.position.y ;
    }

    private void OnMouseDrag()
    {
        // Get the current mouse position in world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Update the object's position based on the mouse  position and offset
        transform.position = new Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);
    }

    private void OnMouseUp()
    {
        mouseButtonReleased = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string thisGameObjectName ;
        string collisionGameObjectName ;

        // Extract the object name prefix (e.g., "Diano1") from the full object name
        thisGameObjectName=gameObject.name.Substring(0,name.IndexOf("_"));
        collisionGameObjectName=collision.gameObject.name.Substring(0,name.IndexOf("_"));
        if (mouseButtonReleased && thisGameObjectName=="Diano 1" && thisGameObjectName==collisionGameObjectName)
        {
            // Instantiate a new object ("Diano2") at the current position and rotation
            Instantiate(Resources.Load("Diano 2_Object"), transform.position, Quaternion.identity);
             mouseButtonReleased = false;
            // Destroy the colliding objects
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
            // Reset the mouseButtonReleased flag
           
        }
        else if (mouseButtonReleased && thisGameObjectName=="Diano 2" && thisGameObjectName==collisionGameObjectName)
        {
            // Instantiate a new object ("Diano2") at the current position and rotation
            Instantiate(Resources.Load("Diano 3_Object"), transform.position, Quaternion.identity);
             mouseButtonReleased = false;
            // Destroy the colliding objects
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
            // Reset the mouseButtonReleased flag
           
        }
    }
}
