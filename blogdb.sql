-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.6.40-log - MySQL Community Server (GPL)
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- 导出 blogdb 的数据库结构
CREATE DATABASE IF NOT EXISTS `blogdb` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `blogdb`;

-- 导出  表 blogdb.account 结构
CREATE TABLE IF NOT EXISTS `account` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Account` varchar(20) NOT NULL COMMENT '账号',
  `Name` varchar(30) NOT NULL COMMENT '昵称',
  `Pwd` varchar(30) NOT NULL COMMENT '密码，MD5加密',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Account` (`Account`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='后台用户表';

-- 数据导出被取消选择。
-- 导出  表 blogdb.article 结构
CREATE TABLE IF NOT EXISTS `article` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryId` int(11) NOT NULL COMMENT '分类Id',
  `AccountId` int(11) NOT NULL COMMENT '用户Id',
  `Title` varchar(200) NOT NULL COMMENT '标题',
  `Content` longtext NOT NULL COMMENT '内容',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8 COMMENT='文章表';

-- 数据导出被取消选择。
-- 导出  表 blogdb.category 结构
CREATE TABLE IF NOT EXISTS `category` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(50) NOT NULL COMMENT '名称',
  `Desc` varchar(200) DEFAULT NULL COMMENT '描述',
  `Sequence` int(11) NOT NULL COMMENT '排序',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8 COMMENT='分类表';

-- 数据导出被取消选择。
-- 导出  表 blogdb.image 结构
CREATE TABLE IF NOT EXISTS `image` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryId` int(11) NOT NULL COMMENT '分类Id',
  `FilePath` varchar(200) NOT NULL COMMENT '文件路径',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8 COMMENT='图片表';

-- 数据导出被取消选择。
-- 导出  表 blogdb.imagecategory 结构
CREATE TABLE IF NOT EXISTS `imagecategory` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(50) NOT NULL COMMENT '名称',
  `Desc` varchar(200) DEFAULT NULL COMMENT '描述',
  `Sequence` int(11) NOT NULL COMMENT '排序值，越大越靠前',
  `CreateTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='图片分类';

-- 插入管理员账号
INSERT INTO `account` (`Account`, `Name`, `Pwd`, `CreateTime`) VALUES ('admin', 'admin', '123456', sysdate());

-- 数据导出被取消选择。
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
