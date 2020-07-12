#pragma once
class IModule
{
public:
	virtual bool Init() = 0;
    virtual bool Update() = 0;
    virtual bool Shut() = 0;
};