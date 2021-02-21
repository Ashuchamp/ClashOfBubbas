using System;
using System.Collections;
using System.Collections.Generic;

class Platform
{
    //# times the player touches the platform
    private int numTimesTouched;
    //The location of the platform relative to the upper left corner of the screen
    private Vector2 vector;
    //Platform textures
    readonly Texture Tplat1 = Engine.LoadTexture("plat.png");
    readonly Texture customPlatT = Engine.LoadTexture("plat1.png");
    //Constructor
    public Platform(Vector2 v)
    {
        vector = v;
    }

    /**
     * Get the location of the platform 
     * return the field vector
     */
    public Vector2 getVector()
    {
        return vector;
    }

    /**
     * Change the location of the platform
     * @param vNew: the designated new location of the platform (as a vector)
     */
    public void modifyVector(Vector2 vNew)
    {
        vector = vNew;
    }

    /**
     * Draw the platform based on the type given
     * @param type: the indicated type of platform (String)
     */
    public void drawPlatform(String type)
    {
        if (type.Equals("normal"))
        {
            Engine.DrawTexture(customPlatT, vector);
        }
    }

    /**
     * Check if the player is touching the platform
     * @param charVec: location of the player (vector)
     */
    public bool hittingPlatform(Vector2 charVec)
    {
        if (Math.Abs(charVec.X - vector.X) <= 40 && Math.Abs(charVec.Y - vector.Y) <= 29)
        {
            numTimesTouched++;
            return true;
        }
        return false;
    }

    /**
     * Return the number of times the player has touched the platform
     * return integer representing the # times the player touched the platform so far
     */
    public int timesTouchedPlatform()
    {
        return numTimesTouched;
    }
}