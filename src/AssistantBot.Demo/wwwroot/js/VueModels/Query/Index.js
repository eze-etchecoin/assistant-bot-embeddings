// Esto era en Vue 2
//const vm = new Vue({
//    el: '#vueContainer',
//    data: {
//        Texto: ''
//    }
//})

const { createApp } = Vue;

createApp({
    data() {
        return {
            Texto: "",
            textoIngresado: "",
            mostrarRespuesta: false
        };
    },
    computed: {
        isButtonDisabled() {
            return this.Texto === '';
        }
    },
    methods: {
        consultar() {
            this.mostrarRespuesta= true;
            this.textoIngresado = this.Texto;
            this.Texto = "";

            /* // PETICIÓN A LA API

            const dataObject = {
                Question: this.Texto
            };

            const { data } = axios.post(
                `${ApiUrl}/AssistantBot/AskToKnowledgeBase`,
                dataObject);

            this.textoIngresado = data;
            this.mostrarRespuesta = true;

            */
        },
        
    }
}).mount("#vueContainer");

