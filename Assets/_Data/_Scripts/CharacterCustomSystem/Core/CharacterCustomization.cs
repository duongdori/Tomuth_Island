using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


namespace DR.CharacterCustomSystem
{
    public class CharacterCustomization : MonoBehaviour
    {
        #region Character Object List

        [SerializeField] private List<GameObject> enabledObjects = new List<GameObject>();

        [SerializeField] private CharacterObjectGroups male;

        [SerializeField] private CharacterObjectGroups female;

        [SerializeField] private CharacterObjectGroups currentGender;

        //[SerializeField] private CharacterObjectListsAllGender allGender;
        
        [SerializeField] private List<CustomizationElement> allElements;
        [SerializeField] private List<CustomizationColorElement> allColorElements;
        
        private Gender _gender;
        
        
        #endregion

        #region Camera Variables

        [SerializeField] private Transform camHolder;
        private Camera cam;
        [SerializeField] private float currentFOV;

        private float x = 16;

        private float y = -30;

        #endregion

        [SerializeField] private ElementGroup elementGroup;

        private void Awake()
        {
            cam = Camera.main;
            
        }

        private void Start()
        {
            BuildLists();

            ClearEnableObjects();

            SetupCamera();
        }

        private void Update()
        {
            
            UpdateElementGroup();
            
            if (camHolder)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    x += 1 * Input.GetAxis("Mouse X");
                    y -= 1 * Input.GetAxis("Mouse Y");
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    //y = Mathf.Clamp(y, -45, 15);
                    //camHolder.eulerAngles = new Vector3(y, x, 0.0f);
                }
                else
                {
                    x -= 1 * Input.GetAxis("Horizontal");
                    y -= 1 * Input.GetAxis("Vertical");
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
        }

        private void LateUpdate()
        {
            if (!Input.GetKey(KeyCode.Mouse1)) return;
            if (camHolder)
            {
                y = Mathf.Clamp(y, -45, 15);
                camHolder.eulerAngles = new Vector3(y, x, 0.0f);
            }
        }

        private void SetupCamera()
        {
            if (cam == null) return;
            
            // cam.transform.position = transform.position + new Vector3(0, 0.3f, 2);
            // cam.transform.rotation = Quaternion.Euler(0, -180, 0);
            //camHolder.position = new Vector3(0, 1, 0);
            //cam.LookAt(camHolder);
        }

        public void SetDefault()
        {
            ClearEnableObjects();
            currentGender = null;
        }

        public void SetElementGroup(ElementGroup element)
        {
            if(elementGroup == element) return;
            elementGroup = element;
        }
        private void UpdateElementGroup()
        {
            if(cam == null) return;
            if(camHolder == null) return;
            float smoothSpeed = 5f;
            float yPos = 0f;
            switch (elementGroup)
            {
                case ElementGroup.FullBody:
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, smoothSpeed * Time.deltaTime);
                    yPos = Mathf.Lerp(camHolder.position.y, 31f, smoothSpeed * Time.deltaTime);
                    camHolder.position = new Vector3(-12f, yPos, 190f);
                    break;
                case ElementGroup.Head:
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 20f, smoothSpeed * Time.deltaTime);
                    yPos = Mathf.Lerp(camHolder.position.y, 31.55f, smoothSpeed * Time.deltaTime);
                    camHolder.position = new Vector3(-12f, yPos, 190f);
                    break;
                case ElementGroup.UpperBody:
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 35f, smoothSpeed * Time.deltaTime);
                    yPos = Mathf.Lerp(camHolder.position.y, 31f, smoothSpeed * Time.deltaTime);
                    camHolder.position = new Vector3(-12f, yPos, 190f);
                    break;
                case ElementGroup.LowerBody:
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 40f, smoothSpeed * Time.deltaTime);
                    yPos = Mathf.Lerp(camHolder.position.y, 30.55f, smoothSpeed * Time.deltaTime);
                    camHolder.position = new Vector3(-12f, yPos, 190f);
                    break;
            }
        }

        private void ClearEnableObjects()
        {
            if (enabledObjects.Count != 0)
            {
                foreach (GameObject g in enabledObjects)
                {
                    g.SetActive(false);
                }
            }

            enabledObjects.Clear();
        }

        public void RemoveEnableObject(GameObject obj)
        {
            enabledObjects.Remove(obj);
        }
        public void SetGender(Gender gender)
        {
            _gender = gender;

            switch (gender)
            {
                case Gender.Male:
                    currentGender = male;
                    break;
                case Gender.Female:
                    currentGender = female;
                    break;
            }
        }

        public void UpdateElements()
        {
            if(allElements.Count <= 0) return;
            
            Type type = typeof(CharacterObjectGroups);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].FieldType == typeof(List<GameObject>))
                {
                    List<GameObject> gameObjects = (List<GameObject>)fields[i].GetValue(currentGender);
                    allElements[i].SetElements(gameObjects);
                    allElements[i].SetCharacterCustomizer(this);
                }
            }

            // foreach (CustomizationColorElement colorElement in allColorElements)
            // {
            //     colorElement.SetColorElement();
            // }
            
            
            // allElements[allElements.Count - 1].SetElements(allGender.all_Hair);
            // allElements[allElements.Count - 1].SetCharacterCustomizer(this);
        }

        public void InitBaseCharacter()
        {
            switch (_gender)
            {
                case Gender.Male:
                    //ActivateItem(allGender.all_Hair[0]);
                    ActivateItem(male.headAllElements[0]);
                    ActivateItem(male.eyebrow[0]);
                    ActivateItem(male.facialHair[0]);
                    ActivateItem(male.torso[0]);
                    // ActivateItem(male.armUpperRight[0]);
                    // ActivateItem(male.armUpperLeft[0]);
                    // ActivateItem(male.armLowerRight[0]);
                    // ActivateItem(male.armLowerLeft[0]);
                    // ActivateItem(male.handRight[0]);
                    // ActivateItem(male.handLeft[0]);
                    ActivateItem(male.hips[0]);
                    ActivateItem(male.legRight[0]);
                    ActivateItem(male.legLeft[0]);
                    ActivateItem(male.hair[0]);
                    return;
                
                case Gender.Female:
                    //ActivateItem(allGender.all_Hair[0]);
                    ActivateItem(female.headAllElements[0]);
                    ActivateItem(female.eyebrow[0]);
                    ActivateItem(female.torso[0]);
                    // ActivateItem(female.armUpperRight[0]);
                    // ActivateItem(female.armUpperLeft[0]);
                    // ActivateItem(female.armLowerRight[0]);
                    // ActivateItem(female.armLowerLeft[0]);
                    // ActivateItem(female.handRight[0]);
                    // ActivateItem(female.handLeft[0]);
                    ActivateItem(female.hips[0]);
                    ActivateItem(female.legRight[0]);
                    ActivateItem(female.legLeft[0]);
                    ActivateItem(female.hair[0]);
                    break;
            }
        }

        public void ActivateItem(GameObject go)
        {
            go.SetActive(true);

            enabledObjects.Add(go);
        }

        private Color ConvertColor(int r, int g, int b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1);
        }

        private bool GetPercent(int pct)
        {
            bool p = false;
            int roll = Random.Range(0, 100);
            if (roll <= pct)
            {
                p = true;
            }

            return p;
        }

        private void BuildLists()
        {
            BuildList(male.headAllElements, "Male_Head_All_Elements");
            BuildList(male.eyebrow, "Male_01_Eyebrows");
            BuildList(male.facialHair, "Male_02_FacialHair");
            BuildList(male.torso, "Male_03_Torso");
            // BuildList(male.armUpperRight, "Male_04_Arm_Upper_Right");
            // BuildList(male.armUpperLeft, "Male_05_Arm_Upper_Left");
            // BuildList(male.armLowerRight, "Male_06_Arm_Lower_Right");
            // BuildList(male.armLowerLeft, "Male_07_Arm_Lower_Left");
            // BuildList(male.handRight, "Male_08_Hand_Right");
            // BuildList(male.handLeft, "Male_09_Hand_Left");
            BuildList(male.hips, "Male_10_Hips");
            BuildList(male.legRight, "Male_11_Leg_Right");
            BuildList(male.legLeft, "Male_12_Leg_Left");
            BuildList(male.hair, "Male_13_Hair");

            BuildList(female.headAllElements, "Female_Head_All_Elements");
            BuildList(female.eyebrow, "Female_01_Eyebrows");
            BuildList(female.facialHair, "Female_02_FacialHair");
            BuildList(female.torso, "Female_03_Torso");
            // BuildList(female.armUpperRight, "Female_04_Arm_Upper_Right");
            // BuildList(female.armUpperLeft, "Female_05_Arm_Upper_Left");
            // BuildList(female.armLowerRight, "Female_06_Arm_Lower_Right");
            // BuildList(female.armLowerLeft, "Female_07_Arm_Lower_Left");
            // BuildList(female.handRight, "Female_08_Hand_Right");
            // BuildList(female.handLeft, "Female_09_Hand_Left");
            BuildList(female.hips, "Female_10_Hips");
            BuildList(female.legRight, "Female_11_Leg_Right");
            BuildList(female.legLeft, "Female_12_Leg_Left");
            BuildList(female.hair, "Female_13_Hair");

            //BuildList(allGender.all_Hair, "All_01_Hair");
            // BuildList(allGender.all_Head_Attachment, "All_02_Head_Attachment");
            // BuildList(allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
            // BuildList(allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
            // BuildList(allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair");
            // BuildList(allGender.chest_Attachment, "All_03_Chest_Attachment");
            // BuildList(allGender.back_Attachment, "All_04_Back_Attachment");
            // BuildList(allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
            // BuildList(allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
            // BuildList(allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
            // BuildList(allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
            // BuildList(allGender.hips_Attachment, "All_09_Hips_Attachment");
            // BuildList(allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right");
            // BuildList(allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left");
            // BuildList(allGender.elf_Ear, "Elf_Ear");
        }

        private void BuildList(List<GameObject> targetList, string characterPart)
        {
            Transform[] rootTransform = gameObject.GetComponentsInChildren<Transform>();
            
            Transform targetRoot = null;

            foreach (Transform t in rootTransform)
            {
                if (t.gameObject.name == characterPart)
                {
                    targetRoot = t;
                    break;
                }
            }

            targetList.Clear();

            for (int i = 0; i < targetRoot.childCount; i++)
            {
                GameObject go = targetRoot.GetChild(i).gameObject;

                go.SetActive(false);

                targetList.Add(go);
            }
        }

        public async Task SaveData()
        {
            await CloudSaveManager.ForceSaveSingleData("PlayerGender", _gender);
            
            foreach (CustomizationElement element in allElements)
            {
                await CloudSaveManager.ForceSaveSingleData(element.name, element.ElementID);
            }
        }

        public async Task LoadData()
        {
            var gender = await CloudSaveManager.RetrieveSpecificData<Gender>("PlayerGender");
            SetGender(gender);
            UpdateElements();
            
            foreach (CustomizationElement element in allElements)
            {
                var elementID = await CloudSaveManager.RetrieveSpecificData<int>(element.name);
                element.ActiveElement(elementID);
            }
        }
    }
}
