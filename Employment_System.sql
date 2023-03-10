DROP Database IF EXISTS `projcet`;

create database `projcet`; 

use `projcet`; 
DROP TABLE IF EXISTS `projects`;

CREATE TABLE `projects` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Pname` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `employee`;

CREATE TABLE `employee` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Surname` varchar(45) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `Password` varchar(45) NOT NULL,
  `Photo` varchar(100) NOT NULL,
  `ProId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `frk1_idx` (`ProId`),
  CONSTRAINT `frk1` FOREIGN KEY (`ProId`) REFERENCES `projects` (`Id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `administrator`;

CREATE TABLE `administrator` (
  `Email` varchar(50) NOT NULL,
  `Password` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `task`;

CREATE TABLE `task` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Tname` varchar(50) NOT NULL,
  `ProId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `idcons` (`ProId`),
  CONSTRAINT `idcons` FOREIGN KEY (`ProId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `employee_task`;

CREATE TABLE `employee_task` (
  `EmpId` int NOT NULL,
  `TaId` int NOT NULL,
  `Completed` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`EmpId`,`TaId`),
  KEY `emp2_idx` (`TaId`),
  CONSTRAINT `frk2` FOREIGN KEY (`EmpId`) REFERENCES `employee` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `frk3` FOREIGN KEY (`TaId`) REFERENCES `task` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;



INSERT INTO `administrator` VALUES ('administrator@gmail.com','admin123');





