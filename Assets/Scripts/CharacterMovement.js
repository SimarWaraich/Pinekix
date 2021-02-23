 #pragma strict
 
 var waypoint : Transform[];            // The amount of Waypoint you want
 var patrolSpeed : float = 3;        // The walking speed between Waypoints
 var loop : boolean = true;            // Do you want to keep repeating the Waypoints
 var dampingLook = 6.0;                // How slowly to turn
 var pauseDuration : float = 0;        // How long to pause at a Waypoint
 var wayPoint_2 : Transform;
 var wayPoint_6 : Transform;
 var SpriteFront : Sprite;
 var SpriteBack : Sprite;

 private var curTime : float;
 private var currentWaypoint : int = 0;
 private var character : CharacterController;
 private var rotateLeft : boolean;
 private var rotateRight : boolean;
 private var sprite_change : SpriteRenderer;
 
 function Start(){
 
     character = GetComponent(CharacterController);
     sprite_change = GetComponent(SpriteRenderer);
     rotateLeft = false;
     rotateRight = false;
 }
 
 function Update(){
 
     if(currentWaypoint < waypoint.length){
         patrol();
         }else{    
     if(loop){
         currentWaypoint=1;
         } 
     }
     if(waypoint[currentWaypoint-1] == wayPoint_2 && !rotateLeft)
     {
     	print("Code for SpriteFront" + currentWaypoint);
     	rotateLeft = true;
     	rotateRight = false;
     	sprite_change.sprite = SpriteFront;
     }
     if(waypoint[currentWaypoint-1] == wayPoint_6 && !rotateRight)
     {
     	print("Code for SpriteBack");
     	rotateRight = true;
     	rotateLeft = false;
     	sprite_change.sprite = SpriteBack;
     }
//     if(currentWaypoint == 6 && rotateLeft)
//     {
//     	rotateLeft = false;
//     	gameObject.transform.position = Vector3(gameObject.transform.position.x * -1, gameObject.transform.position.y, gameObject.transform.position.z);
//     }
 }
 
 function patrol(){
 
         var target : Vector3 = waypoint[currentWaypoint].position;
         target.z = transform.position.z; // Keep waypoint at character's height
         var moveDirection : Vector3 = target - transform.position;
 
     if(moveDirection.magnitude < 0.5){
         if (curTime == 0)
             curTime = Time.time; // Pause over the Waypoint
         if ((Time.time - curTime) >= pauseDuration){
             currentWaypoint++;
             curTime = 0;
         }
     }else{        
//         var rotation = Quaternion.LookRotation(target - transform.position);
//         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
         character.Move(moveDirection.normalized * patrolSpeed * Time.deltaTime);
     }    
 }