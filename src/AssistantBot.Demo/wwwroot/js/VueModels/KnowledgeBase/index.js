

const { createApp } = Vue;
createApp({
    data() {
        return {
            Texto: "",
            isLoading: false,
            showingAlert: false,
        };
    },
    methods: {

        async guardar() {
            
            this.showingAlert = false;
            this.isLoading = true;
            
            const dataObject = {
                Paragraph: this.Texto
            };

            const { data } = await axios.post(
                `${ApiUrl}/knowledgebase/addparagraphtoknowledgebase`,
                dataObject);

            this.isLoading = false;
            this.showingAlert = true;  

        },
        
        //showAlert() {
        //    this.isLoading = false;
        //    this.showingAlert = true;
        //},

        hidenAlert() {
            this.showingAlert = false;
        }

    },
    computed: {
        buttonDisabled() {
            return this.Texto === "";
        }
    }

}).mount('#vueContainer');


