﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonoBehaviour<Player> {
     
    [SerializeField]
    GameObject m_projectile;
    [SerializeField]
    GameObject m_firePos;
    [SerializeField]
    GameObject m_bulletPool;
    [SerializeField]
    GameObject m_invincibleEffect;
    List<Projectile> m_projectileList;
    GameObjectPool<GameObject> m_projectile_Pool;
    Animation m_animation;
    float m_speed = 5f;
    public int m_power { get; set; }
    float m_invincibleDuration = 3f;
    bool m_col_left, m_col_right;
    bool m_clickOn;
    // Use this for initialization
    protected override void OnStart() {
        //base.OnStart();
        int num = 0;
        m_projectile_Pool = new GameObjectPool<GameObject>(10, () =>
        {
            ++num;
            GameObject obj = Instantiate(m_projectile) as GameObject;            
            obj.name = "bullet" + num;            
            obj.transform.parent = m_bulletPool.transform;            
            return obj;
        });
        SoundManager.Instance.PlayBGM(SoundManager.BGM_CLIP.BGM_01);
        m_projectileList = new List<Projectile>();
        m_invincibleEffect.SetActive(false);
        m_animation = GetComponent<Animation>();
        Invoke("OnShoot", 3f);
        m_power = 1;
    }
    void OnShoot()
    {
        // Get object from pool.
        GameObject obj = m_projectile_Pool.pop();
        obj.SetActive(true);
        //obj.transform.parent = m_firePos.transform;
        obj.transform.position = m_firePos.transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        Projectile item = obj.GetComponent<Projectile>();           
        m_projectileList.Add(item);
        Invoke("OnShoot", 0.1f);
    }
    public void RemoveProjectile(Projectile projectile)
    {
        m_projectileList.Remove(projectile);
        projectile.gameObject.SetActive(false);
        projectile.gameObject.transform.parent = m_bulletPool.transform;
        m_projectile_Pool.push(projectile.gameObject);
    }
    public void SetInvincible()
    {
        m_invincibleEffect.SetActive(true);
        m_animation.CrossFade("Invincible");
        Invoke("SetIdle", m_invincibleDuration);
        CancelInvoke("OnShoot");
    }
    public void SetIdle()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Normal);
        m_invincibleEffect.SetActive(false);
        m_animation.CrossFade("fly");
        OnShoot();
    }
    Vector3 startPos;
    Vector3 targetPos;
    Vector3 result;
    void MovePlayer()
    {
        float dir = Input.GetAxis("Horizontal");       
        if (Input.GetMouseButtonDown(0))
        {
            m_clickOn = true;
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);            
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_clickOn = false;
            startPos = Vector3.zero;
        }        
        if (m_clickOn)
        {
            Vector3 mousePos = Input.mousePosition;                
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float reverse = 1.0f;
            dir = (targetPos.x - startPos.x) * reverse;
            if (dir < 0 && m_col_left || dir > 0 && m_col_right) { dir = 0; }
            transform.Translate(new Vector3(dir, 0, 0));
            startPos = targetPos;
        }
       /* if (dir < 0 && m_col_left || dir > 0 && m_col_right) { dir = 0; }
        if (dir == 0) { dir = transform.position.x; }
        //(dir != 0)
        transform.position = new Vector3(dir, transform.position.y);//Vector3.MoveTowards(transform.position, new Vector3(dir, transform.position.y), Mathf.Abs(dir));
        */
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "wall_left") {
            m_col_left = true;
            startPos = result;
        }
        if (collision.tag == "wall_right") {
            m_col_right = true;
            startPos = result;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "wall_left")
        {
            //m_col_left = true;
            startPos = result;
        }
        if (collision.tag == "wall_right")
        {
            //m_col_right = true;
            startPos = result;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "wall_left") {
            m_col_left = false;
            
        }
        if (collision.tag == "wall_right") {
            m_col_right = false;            
        }
    }    
    // Update is called once per frame
    void Update () {
        MovePlayer();                
    }
}
