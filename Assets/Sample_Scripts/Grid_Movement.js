#pragma strict
 
 private var speed = 2.0;
 private var pos : Vector3;
 private var tr : Transform;
 
 function Start() {
     pos = transform.position;
     tr = transform;
 }
 
 function Update() {
 
     if (Input.GetKeyDown(KeyCode.D) && tr.position == pos) {
         pos += Vector3.right;
     }
     else if (Input.GetKeyDown(KeyCode.A) && tr.position == pos) {
         pos += Vector3.left;
     }
     else if (Input.GetKeyDown(KeyCode.W) && tr.position == pos) {
         pos += Vector3.up;
     }
     else if (Input.GetKeyDown(KeyCode.S) && tr.position == pos) {
         pos += Vector3.down;
     }
     
     transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
 }   