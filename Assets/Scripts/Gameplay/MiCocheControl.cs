using UnityEngine;
using UnityEngine.UI;

public class MiCocheControl : MonoBehaviour
{

    public Transform fuerzaR;
    public Transform fuerzaL;
    Rigidbody car_Rigidbody;
    public float multiplier = 35000f;

    public Slider sliderR;
    public Slider sliderL;
    public ParticleSystem chispas;
    bool particlesPlay = false;

    void Start()
    {
        car_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // esta primera seccion es necesaria por usar la raiz cuadrada para intensificar los valores de las fuerzas cercanas a 0, y por no poder aplicar raiz a valores negativos
        short signoR = 1;
        short signoL = 1;
        if (sliderR.value < 0)
        {
            signoR = -1;
        }
        if (sliderL.value < 0)
        {
            signoL = -1;
        }

        // aplico las fuerzas en los laterales del coche chocador, dependiendo del valor de cada slider en pantalla
        car_Rigidbody.AddForceAtPosition(this.transform.forward * signoR * Mathf.Sqrt(Mathf.Abs(sliderR.value)) * multiplier * Time.deltaTime, fuerzaR.transform.position);
        car_Rigidbody.AddForceAtPosition(this.transform.forward * signoL * Mathf.Sqrt(Mathf.Abs(sliderL.value)) * multiplier * Time.deltaTime, fuerzaL.transform.position);

        // activar las particulas solo si el coche camina
        // calcular velocidad del coche
        Vector3 velCoche = car_Rigidbody.GetPointVelocity(transform.TransformPoint(this.transform.position));
       
        if (Mathf.Abs(velCoche.x) < 2.0f && Mathf.Abs(velCoche.z) < 2.0f)
        {
            StopChispas();
        }
        else if (!particlesPlay)
        {
            PlayChispas();
            particlesPlay = true;
        }
    }

    void PlayChispas()
    {
        chispas.Play();
    }

    void StopChispas()
    {
        chispas.Stop();
        particlesPlay = false;
    }
}
