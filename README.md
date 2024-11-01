# SubmitProject2
 
1-1 차이점
입문
이벤트를 구독시켜놓고 ( += Move; )
키에 대응되는 입력을 받았을때 Send Messages 로
알림?을 다 뿌려놓고 알림에 맞는 
이벤트의 구독되어 있는 애가 실행되게 됨

숙련
Move에 대한것을 정의해놓고 매 FixedUpdate마다 실행을 하고있음(실행은 되지만 입력이 없어서 동작X)
OnMove를 버튼클릭이벤트 처럼 등록하고,
키에 대응되는 값이 들어올경우 curMovementInput = context.ReadValue<Vector2>(); 에 값을 넣어줌
매 FixedUpdate에 실행되고 있던 Move가 값이 들어오기때문에 동작함

공통점
인풋액션에 대응되는 애를 설정해놔야한다




1-2 CharacterManager 와 Player의 역할
CharacterManager는 강의기준으로선 플레이어 인스턴스에 대한 싱글톤만 사용중
Player는 주체적으로 행동을 결정하며, 스탯이나 아이템등 주로 게임을 진행해나가는데 필요한 데이터 정보들을 가지고 있다





1-3 Move, Cameralook , isGrounded의 핵심로직 분석
Move는 1-1에 서술

CameraLook 은 y축에 대하여 회전이 우리가 생각하는 좌우를 보는것으로 camCurXRot += mouseDelta.y를 사용하였고,
camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); 을 통하여 위아래 최대범위를 정해주었으며,
cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);  rotation을 돌려볼경우, 음수여야 위쪽을 보기때문에 음수를 준것이다
카메라는 플레이어에 달려있는 자식오브젝트기 때문에 localEulerAngles를 사용하였다.
transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity , 0); 위아래를 보는건 X축에 대비하여 회전이기때문에 x에 y값을 넣은것이고,
위아래와 달리 플레이어 자체가 좌우로 몸이 돌려져야하기때문에 플레이어 자체의 transform 을 rotation 한다.

isGrounded는 4개의 레이를 짧게 쏴서 레이가 닿은 면이 땅인지 레이어마스크를 체크하고, 땅이면 점프 등에 제약을 걸기위해 bool값을 반환한다.




1-4 Move와 CameraLock 함수를 각각 FixedUpdate, LateUpdate에서 호출하는 이유
Move는 rigidbody가 동작하게 되는 물리적인 동작이기때문에 FixedUpdate에서 호출
Update는 매 프레임마다 동작하기때문에 컴퓨터 성능에따라 매 프레임이 달라서 호출이 유동
고정된 시간 간격으로 호출하는 FixedUpdate에서 호출하는것

CameraLock은 플레이어 동작에 있어서 중요한 로직을 담당하는 애는 아니라서
동작의 우선순위가 딱히 필요 없기때문에 맨 마지막에 호출되는 LateUpdate 라고 생각중 ..





2-1 별도의 UI 스크립트를 만드는 이유
프로젝트가 진행됨에 따라 UI별로 추가적으로 필요하거나 불필요한 부분을 제거할때
보수하기 쉽게 접근하기 위해 별도의 스크립트를 만든다





2-2 인터페이스의 특징 / 로직 분석
인터페이스의 특징: 비슷한 카테고리에 대해 공통적인 부분을 작성할때 사용한다
이때 상속받은 클래스는 인터페이스의 부분을 무조건 구현하여야 하며,
인터페이스는 추상클래스이기때문에 내용 구현을 할 필요는 없다.
인터페이스는 다른 추상클래스와 다르게 다중 상속이 가능하다.

로직 분석: IInteractable 을 상속받는 ItemObject를 만들어 구현이 필요한 내용을 작성하고,
Interaction.cs 에서 curInteractGameObject = hit.collider.gameObject; 를 하여 ItemObject가 달려있는
게임 오브젝트를 찾은 뒤, curInteractable = hit.collider.GetComponent<IInteractable>(); 를 하여
컴포넌트를 찾아 사용한다.





2-3
UICondition 구조
플레이어에 할당되어있던 health, stat 등등을 캔버스 UI를 통해 표시해주게 된다

CampFire
닿은 collider를 OnTriggerEnter에서 things 에 배열로 추가하고, 배열에 추가된 things는 
start 부분에서 InvokeRepeating 으로 damageRate 의 시간마다 DealDamage 를 실행하게 된다.

DamageIndicator
플레이어가 피격당했을 경우 코루틴을 사용하여 지정된 색에서 점차 사라지는 이펙트를 연출한다




3-1 Interaction의 구조와 핵심로직
Interaction을 달고있는 플레이어가 ray를 maxCheckDistance만큼 쏘고다니다가, 인터랙트 가능한 오브젝트가 ray에 걸리게 되면
해당 오브젝트를 체크하여 텍스트를 출력해주는 메서드를 실행해주고,
InputAction.CallbackContext context 를 입력받아 해당 오브젝트와 상호작용을 하게된다.
강의 기준에서의 상호작용은 게임오브젝트와 setactive를 꺼주면서 아이템창에 해당 오브젝트 정보를 넣게된다.





3-2 Inventory의 구조
controller.inventory += Toggle; 로 버튼에 대응되는 키 입력시 InventoryWindow의 SetActive 값을 변경해준다
아이템슬롯을 배열로 만들고 ClearSelectedItemWindow()를 통해 초기화를 한번 해준다.
아이템 데이터가 canStack 이면 인벤토리에 있는지 체크하고 있다면 quantity를 증가시키고 UpdateUI를 진행한다.
canStack 이 아니라면 비어있는 슬롯을 가져오고 단순히 수량1개로 인벤토리에 추가하게 되며
비어있는 슬롯이 없다면 아이템을 버리는 ThrowItem 메서드를 실행한다.
ThrowItem은 Instantiate를 다시 실행하는 메서드
아이템을 선택하게 되면 ItemSlot.cs 에서 온클릭버튼으로 SelectItem 메서드가 실행되고
해당 아이템의 이름,정보 등을 text쪽에 뿌려주며, 아이템의 타입에 따라 UseButton,EquipButton 등 각기 다른 버튼을 SetActive True로 바꿔준다.
OnUseButton 메서드를 실행하면 아이템의 정해진 ITemDataConsumable 에 따라서 Heal 또는 Eat 메서드가 실행되며 해당 아이템은 삭제되는 메서드인 RemoveSelectedItem()가 실행되게 된다.
OnEquipButton() 메서드는 기존에 착용한 장비가 있다면 UnEquip 메서드가 실행되게 되고 해당 장비를 매개변수로 EquipNew 메서드가 실행되며 장착되게 된다.

