using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { set; get; }


    // List of pieces
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longBlocks = new List<Piece>();
    public List<Piece> jumpBlocks = new List<Piece>();
    public List<Piece> slideBlocks = new List<Piece>();
    public List<Piece> pieces = new List<Piece>(); // todas las piezas en la lista


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Level Spawning
    public Piece GetPiece(PieceType pt, int visualIndex)
    {
        Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if (p == null)
        {
            GameObject go = null;

            if(pt == PieceType.ramp)
            {
                go = ramps[visualIndex].gameObject;
            }
            else if (pt == PieceType.longblock)
            {
                go = longBlocks[visualIndex].gameObject;
            }
            else if (pt == PieceType.jumpBlock)
            {
                go = jumpBlocks[visualIndex].gameObject;
            }
            else if (pt == PieceType.slideBlock)
            {
                go = slideBlocks[visualIndex].gameObject;
            }

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }

        return p;
    }
    

}
