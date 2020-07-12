/*
 Navicat Premium Data Transfer

 Source Server         : Game1
 Source Server Type    : SQLite
 Source Server Version : 3021000
 Source Schema         : main

 Target Server Type    : SQLite
 Target Server Version : 3021000
 File Encoding         : 65001

 Date: 29/05/2020 21:10:05
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
-- Records of ITEM
-- ----------------------------
INSERT INTO "ITEM" VALUES (2, 2, 3.0, 4.0);
INSERT INTO "ITEM" VALUES (3, 2, 3.0, 4.0);
INSERT INTO "ITEM" VALUES (5, 8, 20.0, 10.0);
INSERT INTO "ITEM" VALUES (6, 6, 3.0, 4.0);
INSERT INTO "ITEM" VALUES (4, 8, 20.0, 20.0);

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
-- Records of LOGIN
-- ----------------------------
INSERT INTO "LOGIN" VALUES ('admin', 123456);
INSERT INTO "LOGIN" VALUES ('admin2', 45678910);
INSERT INTO "LOGIN" VALUES (1222, 630);
INSERT INTO "LOGIN" VALUES ('lyh', 123456);
INSERT INTO "LOGIN" VALUES ('test2', 12345);
INSERT INTO "LOGIN" VALUES ('test', 123456);
INSERT INTO "LOGIN" VALUES (111, 111111);
INSERT INTO "LOGIN" VALUES ('test3', 123456);
INSERT INTO "LOGIN" VALUES ('test4', 123456);

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
-- Records of POINT
-- ----------------------------
INSERT INTO "POINT" VALUES (1, 2);
INSERT INTO "POINT" VALUES (4, 1);
INSERT INTO "POINT" VALUES (5, 1);
INSERT INTO "POINT" VALUES (10, 10);
INSERT INTO "POINT" VALUES (8, 2);
INSERT INTO "POINT" VALUES (9, 1);

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
