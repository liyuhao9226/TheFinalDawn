using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Enemy Enemy;
    public HeroBasic HeroBasic;
    public Platform Platform;
    public Core Core;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy != null) { GetComponent<Slider>().value = Enemy.hp; GetComponent<Slider>().maxValue = Enemy.maxHp; }
        if(HeroBasic!= null) { GetComponent<Slider>().value = HeroBasic.hp; GetComponent<Slider>().maxValue = HeroBasic.maxHp; }
        if (Platform != null) { GetComponent<Slider>().value = Platform.hp; GetComponent<Slider>().maxValue = Platform.maxHp; }
        if (Core != null) { GetComponent<Slider>().value = Core.hp; GetComponent<Slider>().maxValue = Core.maxHp; }
    }


}
