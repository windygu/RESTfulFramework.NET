-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        10.1.9-MariaDB - mariadb.org binary distribution
-- 服务器操作系统:                      Win64
-- HeidiSQL 版本:                  9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 restfulframework 的数据库结构
CREATE DATABASE IF NOT EXISTS `restfulframework` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `restfulframework`;


-- 导出  表 restfulframework.sys_config 结构
CREATE TABLE IF NOT EXISTS `sys_config` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `key` varchar(255) NOT NULL,
  `value` varchar(255) NOT NULL,
  `remark` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 COMMENT='系统配置。';

-- 正在导出表  restfulframework.sys_config 的数据：~10 rows (大约)
/*!40000 ALTER TABLE `sys_config` DISABLE KEYS */;
INSERT INTO `sys_config` (`id`, `key`, `value`, `remark`) VALUES
	(1, 'sms_account', 'szyf@szyf', '短信帐号'),
	(2, 'sms_password', '&:OpF!8N', '短信密码'),
	(3, 'sms_code_content', '验证码:{code}', '短信验证码内容'),
	(4, 'account_secret_key', '123456', '用于验证帐号的密钥'),
	(5, 'redis_address', '127.0.0.1', 'redis服务器地址'),
	(6, 'redis_port', '6379', 'redis服务器端口'),
	(7, 'getui_host', 'http://sdk.open.api.igexin.com/apiex.htm', '推送配置'),
	(8, 'getui_appkey', 'nKlirhKeSR967KBpN1HO47', '推送配置'),
	(9, 'getui_mastersecret', '7pz4l72l3S7iY1wYlI9049', '推送配置'),
	(10, 'getui_appid', 'jlapzlMgS99lcYpzgDiAD', '推送配置');
/*!40000 ALTER TABLE `sys_config` ENABLE KEYS */;


-- 导出  表 restfulframework.user 结构
CREATE TABLE IF NOT EXISTS `user` (
  `id` char(36) NOT NULL COMMENT '帐号唯一id',
  `account_name` varchar(255) NOT NULL COMMENT '登陆帐号',
  `passwrod` varchar(255) NOT NULL COMMENT '登陆密码{NoAllow}',
  `account_type` varchar(255) NOT NULL COMMENT '帐号类型',
  `realname` varchar(255) NOT NULL COMMENT '真实姓名',
  `create_time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户基础信息。';

-- 正在导出表  restfulframework.user 的数据：~8 rows (大约)
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`id`, `account_name`, `passwrod`, `account_type`, `realname`, `create_time`) VALUES
	('3a8fe3ab-6491-4c46-8694-0f835a6f752a', '13266649882', '123456', '手机', '任佳卓', '2016-01-05 20:08:27'),
	('4c26903c-deb5-4873-a60b-70fe5e31a51b', '13800138001', '123456', '手机', '韦诗韵', '2016-01-07 16:06:59'),
	('51f0496d-762d-4de9-a1b9-10b7b1eaaa85', 'waitaction123', '123456', '手机', 'aaa', '2016-02-26 18:56:25'),
	('6343a669-132d-4e74-90af-e32dcdeb6cdb', '18024454137', '123456', '手机', '陈启精', '2016-01-04 11:42:24'),
	('7976d63b-f7cb-45f9-a107-24eae64ee72c', '18682260787', '123456', '手机', '韩熠', '2016-01-07 16:07:35'),
	('845ffb97-e0ce-4bbe-b2d8-40745b2bc17b', '13888138888', '123456', '手机', '黄国君', '2016-01-07 16:08:20'),
	('afaadbb8-dc3a-4939-8c98-4b547b4a7303', '13510421643', '123456', '手机', '蔡翔', '2016-01-05 20:05:54'),
	('df9ea2e4-e8a8-4d5c-aed1-349ec664ab7d', '13800138003', '123456', '手机', '刘娟娟', '2016-01-07 16:08:20');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
