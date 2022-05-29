using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Controller;

namespace Reaper.Environment
{
    public class Shop : MonoBehaviour
    {
        private bool shopActive;
        [SerializeField] int shopID;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player" || collision.isTrigger)
                return;
            collision.GetComponent<PlayerController>().atShop = shopID;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag != "Player" || collision.isTrigger)
                return;
            collision.GetComponent<PlayerController>().atShop = -1;
        }
    }
}