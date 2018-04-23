using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace qy.config
{
    public class RoleConfig : BaseConfig
    {
        private Dictionary<string, RoleItem> dic = new Dictionary<string, RoleItem>();
        public override string Name()
        {
            return "role.xml";
        }
        /*
        public override bool Read(string xml)
        {
            for (int i = 0; i < 10; i++)
            {
                RoleItem item = new RoleItem()
                {
                    id = i.ToString(),
                    name = "帅哥齐" + i.ToString(),
                    headIcon = "person" + Random.Range(1, 4),
                    introduction = "sssssssssssssddddddddddddfffffffffff",
                    ability = new Ability(Random.Range(10, 90), Random.Range(10, 90), Random.Range(10, 90)),
                };
                list.Add(item);
            }
            return true;
        }
        */
        internal override void ReadItem(XmlElement item)
        {
            RoleItem role = new RoleItem();
            role.id = item.GetAttribute("id");
            role.name = GetLanguage(item.GetAttribute("name"));
            role.introduction = GetLanguage(item.GetAttribute("introduction"));
            role.headIcon = item.GetAttribute("headIcon");
            role.ability = new Ability(int.Parse(item.GetAttribute("discipline")),int.Parse(item.GetAttribute("loyalty")),int.Parse(item.GetAttribute("wisdom")));
            role.questID = item.GetAttribute("quest");
            dic.Add(role.id,role);
        }

        public RoleItem GetItem(string id)
        {
            RoleItem value;
            dic.TryGetValue(id, out value);
            return value;
        }

        public List<RoleItem> GetRoleList()
        {
            return new List<RoleItem>(dic.Values);
        }
    }

    public class RoleItem:Cloneable<RoleItem>
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string id;
        /// <summary>
        /// 角色名称
        /// </summary>
        public string name;
        /// <summary>
        /// 角色头像
        /// </summary>
        public string headIcon = "person1";
        /// <summary>
        /// 角色简介
        /// </summary>
        public string introduction;
        /// <summary>
        /// 角色能力值
        /// </summary>
        public Ability ability = new Ability(0,0,0);
        /// <summary>
        /// 任务
        /// </summary>
        public string questID;
    }
}

