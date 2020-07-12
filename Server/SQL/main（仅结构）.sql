/*
 Navicat Premium Data Transfer

 Source Server         : Game1
 Source Server Type    : SQLite
 Source Server Version : 3021000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3021000
 File Encoding         : 65001

 Date: 29/05/2020 21:11:05
*/

PRAGMA foreign_keys = false;

-- ----------------------------
-- Table structure for BULLET
-- ----------------------------
DROP TABLE IF EXISTS "BULLET";
CREATE TABLE "BULLET" (
  "ID" INT NOT NULL,
  "PLAYERID" INT,
  "LOCATIONX" REAL,
  "LOCATIONY" REAL,
  "DIRECTION" INT,
  "TEAM" INT,
  "TYPE" INT,
  PRIMARY KEY ("ID")
);

-- ----------------------------
-- Table structure for ITEM
-- ----------------------------
DROP TABLE IF EXISTS "ITEM";
CREATE TABLE "ITEM" (
  "ID" INT,
  "TYPE" INT,
  "LOCATIONX" REAL,
  "LOCATIONY" REAL,
  PRIMARY KEY ("ID")
);

-- ----------------------------
-- Table structure for LOGIN
-- ----------------------------
DROP TABLE IF EXISTS "LOGIN";
CREATE TABLE "LOGIN" (
  "USERNAME" TEXT NOT NULL,
  "PASSWORD" TEXT,
  PRIMARY KEY ("USERNAME"),
  UNIQUE ("USERNAME" ASC)
);

-- ----------------------------
-- Table structure for OCCUPY
-- ----------------------------
DROP TABLE IF EXISTS "OCCUPY";
CREATE TABLE "OCCUPY" (
  "PLAYERID" INT,
  "POINTID" INT,
  "PROGRESS" INT,
  PRIMARY KEY ("PLAYERID", "POINTID"),
  FOREIGN KEY ("PLAYERID") REFERENCES "PLAYER" ("ID") ON DELETE NO ACTION ON UPDATE NO ACTION,
  FOREIGN KEY ("POINTID") REFERENCES "POINT" ("ID") ON DELETE NO ACTION ON UPDATE NO ACTION
);

-- ----------------------------
-- Table structure for PLAYER
-- ----------------------------
DROP TABLE IF EXISTS "PLAYER";
CREATE TABLE "PLAYER" (
  "ID" INT NOT NULL,
  "USERNAME" TEXT,
  "ROOM" INT,
  "NUMBER" INT,
  "MASTER" INT,
  "LOCATIONX" REAL,
  "LOCATIONY" REAL,
  "TEAM" INT,
  "DIRECTION" INT,
  "READY" INT,
  "TYPE" INT,
  "DEAD" INT,
  PRIMARY KEY ("ID")
);

-- ----------------------------
-- Table structure for POINT
-- ----------------------------
DROP TABLE IF EXISTS "POINT";
CREATE TABLE "POINT" (
  "ID" INT,
  "TEAM" INT,
  PRIMARY KEY ("ID")
);

-- ----------------------------
-- Table structure for TEAM
-- ----------------------------
DROP TABLE IF EXISTS "TEAM";
CREATE TABLE "TEAM" (
  "ID" INT,
  "POINT" INT,
  "PLAYERAMOUNT" INT,
  PRIMARY KEY ("ID")
);

PRAGMA foreign_keys = true;
