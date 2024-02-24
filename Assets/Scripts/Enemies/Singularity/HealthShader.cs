using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShader : MonoBehaviour
{
    [SerializeField] private Material healthShader;
    [SerializeField] private HealthMetrics healthMetrics;
    private float healthPercentage;
    private float shaderPercentage;
    private float maxHealth;
    private float currentHealth;
    private float healthShaderNumber = 1f;
    // Start is called before the first frame update
    void Start()
    {
        healthShader = GetComponent<Renderer>().material;
        healthMetrics = GetComponentInParent<HealthMetrics>();
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = healthMetrics.maxHealth;
        currentHealth = healthMetrics.currentHealth;

        healthPercentage = (currentHealth / maxHealth) * 100;
        shaderPercentage = healthPercentage / 100.00f;

        healthShader.SetFloat("_Health", healthShaderNumber * shaderPercentage);
    }
}
