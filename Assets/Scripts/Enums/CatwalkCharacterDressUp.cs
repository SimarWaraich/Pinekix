using UnityEngine;
using System.Collections;
using System.Collections.Generic;


 public class CatwalkCharacterDressUp : MonoBehaviour {
    public GameObject Container;
    public GameObject Prefab;
    public Camera ThisCamera;

    public List<string> ChangedDressChars = new List<string>();

    public void InitializeCatwalkDressUp()
    {
        DeleteAllChars();

        foreach (var Char in EventManagment.Instance.SelectedRoommates)
        {
            GameObject Background = Instantiate (Prefab.gameObject, Vector3.zero, Quaternion.identity)as GameObject;

            Background.transform.parent = Container.transform;
            Background.transform.localScale = Vector3.one;

            var Component = Background.GetComponent<CatwalkCharacter> ();

            var _flatMate = Char.GetComponent <Flatmate> ();
            Component.Name = _flatMate.data.Name.Trim ('"');
            Component.thisController = this;

            GameObject CharRep = Instantiate (Char, Vector3.zero, Quaternion.identity)as GameObject;
            CharRep.gameObject.SetActive (true);
            CharRep.transform.parent = Background.transform;
            CharRep.transform.localPosition = new Vector2 (0, -160);
            CharRep.transform.localScale = new Vector2 (40 , 40);
            CharRep.transform.rotation = Quaternion.identity;
            CharRep.transform.localEulerAngles = new Vector3 (0, 0, 0);

            int Layer = LayerMask.NameToLayer ("UI3D");
            CharRep.SetLayerRecursively (Layer);

            CharRep.GetComponent <GenerateMoney> ().enabled = false;
            if(CharRep.GetComponent<RoomMateMovement> ())
                Destroy (CharRep.GetComponent<RoomMateMovement> ());

            //              GameObject.Destroy (CharRep.transform.GetChild (12).gameObject);
            GameObject.Destroy (CharRep.transform.FindChild ("low money").gameObject);
//            ThisCamera.enabled = true;
        }
        ThisCamera.enabled = true;
    }

    public void DeleteAllChars ()
    {
        for (int i = 0; i < Container.transform.childCount; i++) {
            GameObject.Destroy (Container.transform.GetChild (i).gameObject);
        }
        ThisCamera.enabled = false;
    }

    public void OnCharacterSelectedCoop()
    {
        if (EventManagment.Instance.SelectedRoommates.Count == 3)
        {
            ScreenAndPopupCall.Instance.CloseCharacterCamera ();
            ScreenAndPopupCall.Instance.CloseScreen ();
            ScreenAndPopupCall.Instance.CatWalkCharacterDressUp();
           
            ScreenManager.Instance.CharacterSelection.GetComponent<CharacterSelectionController>().ThisCamera.enabled = false;
            ScreenManager.Instance.CharacterSelection.GetComponent<CharacterSelectionController>().DeleteAllChars ();
        }else
        {
            SocietyManager.Instance.ShowPopUp("Please selete atleast 3 flatmates");
        }
    }
}
