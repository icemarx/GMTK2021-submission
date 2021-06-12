using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : PC
{
    public Player player;
    public Ball ball;

    private LineRenderer lineRenderer;
    private List<CableSegment> cableSegments = new List<CableSegment>();
    private float cableSegLen = 0.25f;
    private int segmentLength = 35;
    private float lineWidth = 0.1f;
    
    void Start() {
        this.lineRenderer = GetComponent<LineRenderer>();
        Vector2 cableStartPoint = player.transform.position;

        for(int i = 0; i < segmentLength; i++) {
            cableSegments.Add(new CableSegment(cableStartPoint));
            cableStartPoint.y -= cableSegLen;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawCable();
    }

    private void FixedUpdate() {
        Simulate();
    }

    private void DrawCable() {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] cablePositions = new Vector3[this.segmentLength];
        for(int i = 0; i < this.segmentLength; i++) {
            cablePositions[i] = this.cableSegments[i].posNow;
        }

        lineRenderer.positionCount = cablePositions.Length;
        lineRenderer.SetPositions(cablePositions);
    }

    private void Simulate() {
        for (int i = 0; i < this.segmentLength; i++) {
            CableSegment firstSegment = cableSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            this.cableSegments[i] = firstSegment;
        }

        for(int i = 0; i < 50; i++) {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint() {
        CableSegment firstSegment = this.cableSegments[0];
        firstSegment.posNow = player.transform.position;
        cableSegments[0] = firstSegment;

        for(int i = 0; i < segmentLength - 1; i++) {
            CableSegment firstSeg = cableSegments[i];
            CableSegment secondSeg = cableSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - cableSegLen);
            Vector2 changeDir = Vector2.zero;

            if(dist > cableSegLen) {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            } else if(dist < cableSegLen) {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if(i != 0) {
                firstSeg.posNow -= changeAmount * error;
                cableSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                cableSegments[i + 1] = secondSeg;
            } else {
                secondSeg.posNow += changeAmount;
                cableSegments[i + 1] = secondSeg;
            }
        }
    }

    public struct CableSegment {
        public Vector2 posNow;
        public Vector2 posOld;

        public CableSegment(Vector2 pos) {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

    public override void Hit(float damage) {
        throw new System.NotImplementedException();
    }
}
