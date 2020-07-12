#pragma once

struct MsgHead
{
	unsigned short size;
	unsigned short type;
};

 enum MsgType
 {
	 LOGIN_C2S,
	 LOGIN_S2C,
 };

#define MAX_NAME 32
#define MAX_MSG 255

 struct Msg_Login_C2S : MsgHead
 {
	 Msg_Login_C2S()
	 {
		 size = sizeof(*this);
		 type = LOGIN_C2S;
	 }
	 char szName[MAX_NAME];
 };

 struct Msg_Login_S2C : MsgHead
 {
	 Msg_Login_S2C()
	 {
		 size = sizeof(*this);
		 type = LOGIN_S2C;
	 }

	 unsigned short id;
 };