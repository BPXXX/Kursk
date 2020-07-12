#pragma once

struct MsgHead
{
	unsigned short size;
	unsigned short type;
};

enum MsgType
{
	REGISTER_C2S,
	REGISTER_S2C,
	LOGIN_C2S,
	LOGIN_S2C,
	CREATE_ROOM_C2S,
	CREATE_ROOM_S2C,
	ENTER_ROOM_C2S,
	ENTER_ROOM_S2C,
	CHANGE_STATE_C2S,
	CHANGE_STATE_S2C,
	EXPEL_C2S,
	EXPEL_S2C,
	START_GAME_C2S,
	START_GAME_S2C,
	EXIT_ROOM_C2S,
	EXIT_ROOM_S2C,
	INPUT_C2S,
	OUTPUT_S2C,
	QUNLIAO_C2S,
	EXIT_QUNLIAO_C2S,
	CONTENT_S2C,
	JINYAN_C2S,
	QUXIAOJINYAN_C2S,
	EXIT_C2S,
	HouseID_C2S,
	HouseID_S2C,
	FANGJIANLT_C2S,
	FANGJIANLT_S2C,
	FANGJIANEXIT_C2S,
	SILIAO_C2S,
	TIREN_C2S,
	TIREN_S2C,
	GameStart_C2S,
	CancelStart_C2S,
	GameStart_S2C,
	VEC_C2S,
	VEC_S2C,
	PickSide_C2S,
	PickSide_S2C,
	Ammo_C2S,
	Damage_S2C,
};
class Item
{
public:
	Item()
	{
		float x = 0.0;
		float y = 0.0;
		int kind = 0;
		bool taken = false;
	}
	float x;
	float y;
	int kind;
	bool taken;
};


#define MAX_NAME 32
#define MAX_MSG 255

struct Msg_Register_C2S : MsgHead
{
	Msg_Register_C2S()
	{
		size = sizeof(*this);
		type = REGISTER_C2S;
	}
	char szName[MAX_NAME];
	char szPassword[MAX_NAME];
};

struct Msg_Register_S2C : MsgHead
{
	Msg_Register_S2C()
	{
		size = sizeof(*this);
		type = REGISTER_S2C;
	}
	int result;
};

struct Msg_Login_C2S : MsgHead
{
	Msg_Login_C2S()
	{
		size = sizeof(*this);
		type = LOGIN_C2S;
	}
	char szName[MAX_NAME];
	char szPassword[MAX_NAME];
};

struct Msg_Login_S2C : MsgHead
{
	Msg_Login_S2C()
	{
		size = sizeof(*this);
		type = LOGIN_S2C;
	}

	int result;
};

struct Msg_CreateRoom_C2S : MsgHead
{
	Msg_CreateRoom_C2S()
	{
		size = sizeof(*this);
		type = CREATE_ROOM_C2S;
	}
	unsigned short id;

};

struct Msg_CreateRoom_S2C : MsgHead
{
	Msg_CreateRoom_S2C()
	{
		size = sizeof(*this);
		type = CREATE_ROOM_S2C;
	}
	unsigned short result;
};

struct Msg_EnterRoom_C2S : MsgHead
{
	Msg_EnterRoom_C2S()
	{
		size = sizeof(*this);
		type = ENTER_ROOM_C2S;
	}
	unsigned short id;
};

struct Msg_EnterRoom_S2C : MsgHead
{
	Msg_EnterRoom_S2C()
	{
		size = sizeof(*this);
		type = ENTER_ROOM_S2C;
	}
	unsigned short result;
	int number;

};

struct Msg_ChangeState_C2S : MsgHead
{
	Msg_ChangeState_C2S()
	{
		size = sizeof(*this);
		type = CHANGE_STATE_C2S;
	}
	int result;
	int number1;//玩家之前位置
	int number2;//玩家现在位置
	int type;
	int ready;
};

struct Msg_ChangeState_S2C : MsgHead
{
	Msg_ChangeState_S2C()
	{
		size = sizeof(*this);
		type = CHANGE_STATE_S2C;
	}
	int result;
	int number1;//玩家之前位置
	int number2;//玩家现在位置
	int type;
	int ready;
};

struct Msg_Expel_C2S : MsgHead
{
	Msg_Expel_C2S()
	{
		size = sizeof(*this);
		type = EXPEL_C2S;
	}
	int number;//被踢玩家的位置
	int id;//房间号
};

struct Msg_Expel_S2C : MsgHead
{
	Msg_Expel_S2C()
	{
		size = sizeof(*this);
		type = EXPEL_S2C;
	}
	unsigned short result;
	int number;
};

struct Msg_StartGame_C2S : MsgHead
{
	Msg_StartGame_C2S()
	{
		size = sizeof(*this);
		type = START_GAME_C2S;
	}
	int id;
};

struct Msg_StartGame_S2C : MsgHead
{
	Msg_StartGame_S2C()
	{
		size = sizeof(*this);
		type = START_GAME_S2C;
	}
	unsigned short result;
};

struct Msg_ExitRoom_C2S : MsgHead
{
	Msg_ExitRoom_C2S()
	{
		size = sizeof(*this);
		type = EXIT_ROOM_C2S;
	}
	int id;
	int number;//玩家位置
};

struct Msg_ExitRoom_S2C : MsgHead
{
	Msg_ExitRoom_S2C()
	{
		size = sizeof(*this);
		type = EXIT_ROOM_S2C;
	}
	unsigned short result;
	int number;
};

struct Msg_Input_C2S :MsgHead//用户向服务器发送的消息
{
	Msg_Input_C2S()
	{
		size = sizeof(*this);
		type = INPUT_C2S;
	}
	int szMsg;
	unsigned short id;
};

struct Msg_Qunliao_C2S :MsgHead//用户向服务器发送的群聊消息
{
	Msg_Qunliao_C2S()
	{
		size = sizeof(*this);
		type = QUNLIAO_C2S;
	}
	char szContent[MAX_MSG];
	unsigned short id;
};

struct Msg_Exit_Qunliao_C2S :MsgHead//用户向服务器发送的群聊消息
{
	Msg_Exit_Qunliao_C2S()
	{
		size = sizeof(*this);
		type = EXIT_QUNLIAO_C2S;
	}
	unsigned short id;
};

struct Msg_FANGJIANLT_C2S :MsgHead//用户向服务器发送的群聊消息
{
	Msg_FANGJIANLT_C2S()
	{
		size = sizeof(*this);
		type = FANGJIANLT_C2S;
	}
	char szContent[MAX_MSG];
	char szName[MAX_NAME];
	unsigned short id;
	unsigned int houseid;
};
struct Msg_FANGJIANLT_S2C :MsgHead//用户向服务器发送的群聊消息
{
	Msg_FANGJIANLT_S2C()
	{
		size = sizeof(*this);
		type = FANGJIANLT_S2C;
	}
	char szContent[MAX_MSG];
	char szName[MAX_NAME];
	unsigned short id;
	unsigned short houseid;
};
struct Msg_Content_S2C :MsgHead//服务器向用户发送聊天内容
{
	Msg_Content_S2C()
	{
		size = sizeof(*this);
		type = CONTENT_S2C;
	}
	int MSG;
	char szContent[MAX_MSG];
	char szName[MAX_NAME];
};

struct Msg_Jinyan_C2S :MsgHead//用户向服务器发送禁言ID
{
	Msg_Jinyan_C2S()
	{
		size = sizeof(*this);
		type = JINYAN_C2S;
	}
	unsigned short id;
};

struct Msg_QuxiaoJinyan_C2S :MsgHead//用户向服务器发送取消禁言ID
{
	Msg_QuxiaoJinyan_C2S()
	{
		size = sizeof(*this);
		type = QUXIAOJINYAN_C2S;
	}
	unsigned short id;
};

struct Msg_Exit_C2S :MsgHead//用户向服务器发送的退出
{
	Msg_Exit_C2S()
	{
		size = sizeof(*this);
		type = EXIT_C2S;
	}
	unsigned short id;
};
struct Msg_FANGJIANEXIT_C2S :MsgHead//用户向服务器发送的退出
{
	Msg_FANGJIANEXIT_C2S()
	{
		size = sizeof(*this);
		type = FANGJIANEXIT_C2S;
	}
	unsigned short id;
	unsigned int houseid;
};
struct Msg_Siliao_C2S :MsgHead//用户向服务器发送的私聊消息
{
	Msg_Siliao_C2S()
	{
		size = sizeof(*this);
		type = SILIAO_C2S;
	}
	char szContent[MAX_MSG];
	unsigned short id;
	char revName[MAX_NAME];
};
struct Msg_Tiren_C2S :MsgHead//用户向服务器发送的私聊消息
{
	Msg_Tiren_C2S()
	{
		size = sizeof(*this);
		type = TIREN_C2S;
	}
	int roomid;
	char szName[MAX_NAME];
};
struct Msg_Tiren_S2C :MsgHead//用户向服务器发送的私聊消息
{
	Msg_Tiren_S2C()
	{
		size = sizeof(*this);
		type = TIREN_S2C;
	}
};



struct Msg_Vec_C2S :MsgHead
{
	Msg_Vec_C2S()
	{
		size = sizeof(*this);
		type = VEC_C2S;
	}
	unsigned short id;
	float x;
	float y;
	int direction;

	
};

struct Msg_Vec_S2C :MsgHead
{
	Msg_Vec_S2C()
	{
		size = sizeof(*this);
		type = VEC_S2C;
	}
	unsigned short id;
	float x;
	float y;
	int direction;
	int hidden;
};

struct Msg_StartRequest_C2S :MsgHead
{
	Msg_StartRequest_C2S()
	{
		size = sizeof(*this);
		type = GameStart_C2S;
	}

	unsigned short id;
	char szName[MAX_NAME];
};

struct Msg_StartRequest_S2C :MsgHead
{
	Msg_StartRequest_S2C()
	{
		size = sizeof(*this);
		type = GameStart_S2C;
	}

	unsigned short id;
	char szName[MAX_NAME];
	bool success;
};
struct Msg_CancelStartRequest_C2S :MsgHead
{
	Msg_CancelStartRequest_C2S()
	{
		size = sizeof(*this);
		type = CancelStart_C2S;
	}

	unsigned short id;
	char szName[MAX_NAME];
};
struct Msg_Ammo_C2S :MsgHead
{
	Msg_Ammo_C2S()
	{
		size = sizeof(*this);
		type = Ammo_C2S;
	}

	unsigned short id;
	float x;
	float y;
	int direction;
};
struct Msg_Damage_S2C :MsgHead
{
	Msg_Damage_S2C()
	{
		size = sizeof(*this);
		type = Damage_S2C;
	}

	unsigned short id;
	int damage;
};
