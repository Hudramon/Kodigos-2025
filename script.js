const { createApp } = Vue

// 🌟 CORREÇÃO FINAL: Define a URL base da API com a nova porta 5007
const API_BASE_URL = 'http://localhost:5007';

createApp({
  data() {
    return {
      logado: false, 
      login: { usuario: '', senha: '', erro: false },
      os: { descricao: '', checklist: [], fotoPreview: null },
      mensagemSucesso: ''
    }
  },
  // Esta função roda quando a página carrega
  created() {
    // Verifica no "caderno" do navegador se o usuário já estava logado
    if (sessionStorage.getItem('usuarioLogado') === 'true') {
      this.logado = true;
    }
  },
  methods: {
    fazerLogin() {
      // ATENÇÃO: A senha correta é 'admin123'
      if (this.login.usuario === 'admin' && this.login.senha === 'admin123') {
        // Se o login estiver certo, anota no "caderno" do navegador
        sessionStorage.setItem('usuarioLogado', 'true'); 
        this.logado = true;
        this.login.erro = false;
      } else {
        this.login.erro = true;
        sessionStorage.removeItem('usuarioLogado');
      }
    },
    // Função para sair
    fazerLogout() {
      // Apaga a anotação do "caderno"
      sessionStorage.removeItem('usuarioLogado');
      // Recarrega a página
      window.location.reload();
    },
    carregarFoto(event) {
      const file = event.target.files[0];
      if (file) { this.os.fotoPreview = URL.createObjectURL(file); }
    },
    async salvarOS() {
      const dadosParaApi = {
        descricao: this.os.descricao,
        checklist: this.os.checklist.join(', ')
      };
      
      try {
        // 🎯 USANDO A NOVA PORTA 5007
        const response = await fetch(`${API_BASE_URL}/ordensdeservico`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(dadosParaApi),
        });

        if (!response.ok) { throw new Error('Falha ao salvar'); }

        this.mensagemSucesso = 'Ordem de Serviço salva com sucesso!';
        // Atrasa a limpeza da mensagem por 3 segundos para dar tempo de ler
        setTimeout(() => { this.mensagemSucesso = ''; }, 3000);

        // Limpa o formulário
        this.os.descricao = '';
        this.os.checklist = [];
        this.os.fotoPreview = null;
        // Zera o campo de arquivo para poder enviar a mesma foto de novo se quiser
        document.querySelector('input[type="file"]').value = '';


      } catch (error) {
        alert('Erro ao conectar com a API. Verifique se o terminal com "dotnet run" está aberto.');
      }
    }
  }
}).mount('#app')