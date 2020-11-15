using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lurker : Viewer
{
    public override void setup() {
        base.setup();
        
        mainLurk = 9;
        fillerLurk = 9;
    }

    public override void endDay() {
        if (attitude > 60) {
            if (Random.Range(0, 50) < (Globals.popularity > 40 ? 40 : Globals.popularity)) {
                Globals.subNumber++;
            }
        }
    }
}
