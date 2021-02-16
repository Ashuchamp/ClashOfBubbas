using System;
using System.Collections;
using System.Collections.Generic;

class Platform
{
    private int numTimesTouched;
    private Vector2 vector;
    readonly Texture Tplat1 = Engine.LoadTexture("plat.png");
    readonly Texture customPlatT = Engine.LoadTexture("plat1.png");
    public Platform(Vector2 v)
    {
        vector = v;
    }

    public Vector2 getVector()
    {
        return vector;
    }

    public void modifyVector(Vector2 vNew)
    {
        vector = vNew;
    }

    public void drawPlatform(String type)
    {
        if (type.Equals("normal"))
        {
            Engine.DrawTexture(customPlatT, vector);
        }
    }

    public bool hittingPlatform(Vector2 charVec)
    {
        if (Math.Abs(charVec.X - vector.X) <= 40 && Math.Abs(charVec.Y - vector.Y) <= 29)
        {
            numTimesTouched++;
            return true;
        }
        return false;
    }

    public int timesTouchedPlatform()
    {
        return numTimesTouched;
    }
}