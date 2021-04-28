// using Mirror;
// using UnityEngine;

//     public class PlayerMovementController : NetworkBehaviour
//     {
//         [SerializeField] private float movementSpeed = 5f;
//         [SerializeField] private CharacterController controller = null;

//         private Vector2 previousInput;

//         private Controls controls;
//         private Controls Controls
//         {
//             get
//             {
//                 if (controls != null) { return controls; }
//                 return controls = new Controls();
//             }
//         }

//         public override void OnStartAuthority()
//         {
//             enabled = true;

//             // InputManager.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
//             // InputManager.Controls.Player.Move.canceled += ctx => ResetMovement();

//             Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
//             Controls.Player.Move.canceled += ctx => ResetMovement();
//         }

//         [ClientCallback]
//         private void OnEnable() => Controls.Enable();
//         [ClientCallback]
//         private void OnDisable() => Controls.Disable();

//         [ClientCallback]
//         private void Update() => Move();

//         [Client]
//         private void SetMovement(Vector2 movement) => previousInput = movement;

//         [Client]
//         private void ResetMovement() => previousInput = Vector2.zero;

//         [Client]
//         private void Move()
//         {
//             Vector3 right = controller.transform.right;
//             Vector3 forward = controller.transform.forward;
//             right.y = 0f;
//             forward.y = 0f;

//             Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

//             controller.Move(movement * movementSpeed * Time.deltaTime);
//         }
//     }


using Mirror;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


namespace UnityStandardAssets.Vehicles.Car
{
    //combined from CarUserControl
    [RequireComponent(typeof (CarController))]  
    public class PlayerMovementController : NetworkBehaviour
    {
        //combined from CarUserControl
        private CarController m_Car; // the car controller we want to use

        //[SerializeField] private float movementSpeed = 5f;
        //[SerializeField] private CharacterController controller = null;

        [SyncVar(hook = nameof(OnColorChanged))]
        private Color car;

        private float previousWS;
        private float previousAD;
        private int increment = 1;
        private Boolean powerup = false;
        private float duration = 5f;
        private float time = 0f;

        // private ControlsCar controls;
        // private ControlsCar ControlsCar
        // {
        //     get
        //     {
        //         if (controls != null) { return controls; }
        //         return controls = new ControlsCar();
        //     }
        // }

        public override void OnStartAuthority()
        {
            enabled = true;

            // ControlsCar.Player.Drive.performed += ctx => SetDrive(ctx.ReadValue<float>());
            // ControlsCar.Player.Drive.canceled += ctx => ResetDrive();
            // ControlsCar.Player.Turn.performed += ctx => SetTurn(ctx.ReadValue<float>());
            // ControlsCar.Player.Turn.canceled += ctx => ResetTurn();

            InputManager.Controls.Player.Drive.performed += ctx => SetDrive(ctx.ReadValue<float>());
            InputManager.Controls.Player.Drive.canceled += ctx => ResetDrive();
            InputManager.Controls.Player.Turn.performed += ctx => SetTurn(ctx.ReadValue<float>());
            InputManager.Controls.Player.Turn.canceled += ctx => ResetTurn();

        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            //Change color
            Transform Cartoon_SportCar_B01 = this.transform.Find("Cartoon_SportCar_B01");

            Transform carrosserie = Cartoon_SportCar_B01.transform.Find("carrosserie");

            Transform Animatable = Cartoon_SportCar_B01.transform.Find("Animatable");
            Transform door_front_L = Animatable.transform.Find("door_front_L");
            Transform door_front_R = Animatable.transform.Find("door_front_R");
            Transform Trunk = Animatable.transform.Find("Trunk");
            Transform hood = Animatable.transform.Find("hood");

            car = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            //Debug.Log(carrosserie.GetComponent<MeshRenderer>().material.color);

            carrosserie.GetComponent<MeshRenderer>().material.color = car;
            door_front_L.GetComponent<MeshRenderer>().material.color = car;
            door_front_R.GetComponent<MeshRenderer>().material.color = car;
            Trunk.GetComponent<MeshRenderer>().material.color = car;
            hood.GetComponent<MeshRenderer>().material.color = car;
        }

        // [ClientCallback]
        // private void OnEnable() => ControlsCar.Enable();
        // [ClientCallback]
        // private void OnDisable() => ControlsCar.Disable();

        //combined from CarUserControl
        [ClientCallback]
        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }

        [ClientCallback]
        // private void Update() => Move();
        private void Update(){
            Move();
            if(powerup&&time<duration){
                time += Time.deltaTime;
            }else if(powerup&&time>=duration){
                powerup = false;
                time = 0f;
                increment = 1;
                Debug.Log("end power up!");
            }
        }

        [Client]
        private void SetDrive(float movement)
        {
            previousWS = movement;
        }

        [Client]
        private void ResetDrive()
        {
            previousWS = 0.0f;
        }

        [Client]
        private void SetTurn(float movement)
        {
            previousAD = movement;
        }

        [Client]
        private void ResetTurn()
        {
            previousAD = 0.0f;
        }

        [Client]
        private void Move()
        {
            // Vector3 right = controller.transform.right;
            // Vector3 forward = controller.transform.forward;
            // right.y = 0f;
            // forward.y = 0f;

            // Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

            // controller.Move(movement * movementSpeed * Time.deltaTime);

            float ADInput = InputManager.controls.Player.Turn.ReadValue<float>();
            float WSInput = InputManager.controls.Player.Drive.ReadValue<float>();

            m_Car.MoveCar(ADInput, WSInput*increment, WSInput*increment, 0f);
        }

        private void OnColorChanged(Color oldColor, Color newColor)
        {
                // Debug.Log("Reached OnColorChanged on Player Prefab");
                Transform Cartoon_SportCar_B01 = this.transform.Find("Cartoon_SportCar_B01");

                Transform carrosserie = Cartoon_SportCar_B01.transform.Find("carrosserie");

                Transform Animatable = Cartoon_SportCar_B01.transform.Find("Animatable");
                Transform door_front_L = Animatable.transform.Find("door_front_L");
                Transform door_front_R = Animatable.transform.Find("door_front_R");
                Transform Trunk = Animatable.transform.Find("Trunk");
                Transform hood = Animatable.transform.Find("hood");


                carrosserie.GetComponent<MeshRenderer>().material.color = newColor;
                door_front_L.GetComponent<MeshRenderer>().material.color = newColor;
                door_front_R.GetComponent<MeshRenderer>().material.color = newColor;
                Trunk.GetComponent<MeshRenderer>().material.color = newColor;
                hood.GetComponent<MeshRenderer>().material.color = newColor;
        }
        public void SpeedUp(){
            increment = 10;
            powerup = true;
            time = 0f;
            Debug.Log("speed up start up!!!");
        }

        public void Slowdown(){
            increment = 0;
            powerup = true;
            time = 0f;
            Debug.Log("slow down start up!!!");
        }
    }
}