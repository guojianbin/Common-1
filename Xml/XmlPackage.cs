﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LCW.Framework.Common.Xml
{
    public class XmlPackage
    {
        public XmlDocument CreateXml(string filepath,string version,string encoding,Action<XmlDocument> action)
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration declaration = document.CreateXmlDeclaration(version,encoding, null);
            document.AppendChild(declaration);
            if (action != null)
            {
                action(document);
            }
            try
            {
                document.Save(filepath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return document;
        }

        public XmlDocument CreateXml(string filepath)
        {
            return CreateXml(filepath, "1.0", "UTF-8", null);
        }

        public XmlDocument CreateXml(string filepath, string encoding)
        {
            return CreateXml(filepath, "1.0",encoding,null);
        }

        public XmlNode CreateXmlNode(XmlDocument document,string name,string text)
        {
            XmlNode node = document.CreateNode(XmlNodeType.Element, name, "");
            node.InnerText = text;
            document.AppendChild(node);
            return node;
        }

        public XmlElement CreateXmlElement(XmlNode Node,string name,string data)
        {
            XmlElement element = Node.OwnerDocument.CreateElement(name);
            element.AppendChild(Node.OwnerDocument.CreateCDataSection(@data));
            Node.AppendChild(element);
            return element;
        }
    }
}
    /*运算符/特殊字符                                说明

    /                           此路径运算符出现在模式开头时，表示应从根节点选择。

    //                          从当前节点开始递归下降，此路径运算符出现在模式开头时，表示应从根节点递归下降。

    .                           当前上下文。

    ..                          当前上下文节点父级。

    *                           通配符；选择所有元素节点与元素名无关。（不包括文本，注释，指令等节点，如果也要包含这些节点请用node()函数）

    @                           属性名的前缀。

    @*                          选择所有属性，与名称无关。

    :                           命名空间分隔符；将命名空间前缀与元素名或属性名分隔。

    ( )                         括号运算符(优先级最高)，强制运算优先级。

    [ ]                         应用筛选模式（即谓词，包括"过滤表达式"和"轴（向前/向后）"）。

    [ ]                         下标运算符；用于在集合中编制索引。

    |                           两个节点集合的联合，如：//messages/message/to | //messages/message/cc

    -                           减法。

    div，                       浮点除法。

    and, or                     逻辑运算。

    mod                         求余。

    not()                       逻辑非

    =                           等于

    ！=                         不等于

    特殊比较运算符              < 或者 &lt;

    <= 或者 &lt;=

    > 或者 &gt;

    >= 或者 &gt;=

    需要转义的时候必须使用转义的形式，如在XSLT中，而在XMLDOM的scripting中不需要转义。*/


/*------------------
/

Document Root文档根.

/*

选择文档根下面的所有元素节点，即根节点（XML文档只有一个根节点）

/node()

根元素下所有的节点（包括文本节点，注释节点等）

/text()

查找文档根节点下的所有文本节点

/messages/message

messages节点下的所有message节点

/messages/message[1]

messages节点下的第一个message节点

/messages/message[1]/self::node()

第一个message节点（self轴表示自身，node()表示选择所有节点）

/messages/message[1]/node()

第一个message节点下的所有子节点

/messages/message[1]/*[last()]

第一个message节点的最后一个子节点

/messages/message[1]/[last()]

Error，谓词前必须是节点或节点集

/messages/message[1]/node()[last()]

第一个message节点的最后一个子节点

/messages/message[1]/text()

第一个message节点的所有子节点

/messages/message[1]//text()

第一个message节点下递归下降查找所有的文本节点（无限深度）

/messages/message[1] /child::node()

/messages/message[1] /node()

/messages/message[position()=1]/node()

//message[@id=1] /node()

第一个message节点下的所有子节点

//message[@id=1] //child::node()

递归所有子节点（无限深度）

//message[position()=1]/node()

选择id=1的message节点以及id=0的message节点

/messages/message[1] /parent::*

Messages节点

/messages/message[1]/body/attachments/parent::node()

/messages/message[1]/body/attachments/parent::* /messages/message[1]/body/attachments/..

attachments节点的父节点。父节点只有一个,所以node()和* 返回结果一样。

（..也表示父节点. 表示自身节点）

//message[@id=0]/ancestor::*

Ancestor轴表示所有的祖辈，父，祖父等。

向上递归

//message[@id=0]/ancestor-or-self::*

向上递归,包含自身

//message[@id=0]/ancestor::node()

对比使用*,多一个文档根元素(Document root)

/messages/message[1]/descendant::node()

//messages/message[1]//node()

递归下降查找message节点的所有节点

/messages/message[1]/sender/following::*

查找第一个message节点的sender节点后的所有同级节点，并对每一个同级节点递归向下查找。

//message[@id=1]/sender/following-sibling::*

查找id=1的message节点的sender节点的所有后续的同级节点。

//message[@id=1]/datetime/@date

查找id=1的message节点的datetime节点的date属性

//message[@id=1]/datetime[@date]

//message/datetime[attribute::date]

查找id=1的message节点的所有含有date属性的datetime节点

//message[datetime]

查找所有含有datetime节点的message节点

//message/datetime/attribute::*

//message/datetime/attribute::node()

//message/datetime/@*

返回message节点下datetime节点的所有属性节点

//message/datetime[attribute::*]

//message/datetime[attribute::node()]

//message/datetime[@*]

//message/datetime[@node()]

选择所有含有属性的datetime节点

//attribute::*

选择根节点下的所有属性节点

//message[@id=0]/body/preceding::node()

顺序选择body节点所在节点前的所有同级节点。（查找顺序为：先找到body节点的顶级节点（根节点）,得到根节点标签前的所有同级节点，执行完成后继续向下一级，顺序得到该节点标签前的所有同级节点，依次类推。）

注意：查找同级节点是顺序查找，而不是递归查找。

//message[@id=0]/body/preceding-sibling::node()

顺序查找body标签前的所有同级节点。（和上例一个最大的区别是：不从最顶层开始到body节点逐层查找。我们可以理解成少了一个循环，而只查找当前节点前的同级节点）

//message[@id=1]//*[namespace::amazon]

查找id=1的所有message节点下的所有命名空间为amazon的节点。

//namespace::*

文档中的所有的命名空间节点。（包括默认命名空间xmlns:xml）

//message[@id=0]//books/*[local-name()='book']

选择books下的所有的book节点，

注意：由于book节点定义了命名空间<amazone:book>.若写成//message[@id=0]//books/book则查找不出任何节点。

//message[@id=0]//books/*[local-name()='book' and namespace-uri()='http://www.amazon.com/books/schema']

选择books下的所有的book节点，(节点名和命名空间都匹配)

//message[@id=0]//books/*[local-name()='book'][year>2006]

选择year节点值>2006的book节点

//message[@id=0]//books/*[local-name()='book'][1]/year>2006

指示第一个book节点的year节点值是否大于2006.

返回xs:boolean: true
-------------------------------------------------------------------------*/