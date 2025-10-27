import os

# --- CONFIGURAÇÃO ---
arquivos_para_incluir = [
    "index.html",
    "style.css",
    "script.js",
    os.path.join("ApiOS", "Program.cs"), 
    "Dockerfile",
    ".gitignore",
    "README.md"
]

# 2. Defina o nome do arquivo de saída.
nome_do_arquivo_de_saida = "analise_do_projeto.md"

# 3. Mapeamento de extensões de arquivo para a linguagem do Markdown 
mapa_de_linguagens = {
    ".html": "html",
    ".css": "css",
    ".js": "javascript",
    ".cs": "csharp",
    ".py": "python",
    ".md": "markdown",
    ".dockerfile": "dockerfile",
    ".gitignore": "gitignore"
}

# --- LÓGICA DO SCRIPT ---

with open(nome_do_arquivo_de_saida, 'w', encoding='utf-8') as arquivo_de_saida:
    print(f"Criando o arquivo '{nome_do_arquivo_de_saida}'...")
    
    arquivo_de_saida.write("# Análise Completa do Projeto\n\n")
    
    
    for i, caminho_do_arquivo in enumerate(arquivos_para_incluir):
        try:
            
            with open(caminho_do_arquivo, 'r', encoding='utf-8') as arquivo_de_entrada:
                conteudo = arquivo_de_entrada.read()
                
                
                extensao = os.path.splitext(caminho_do_arquivo)[1].lower()
                linguagem = mapa_de_linguagens.get(extensao, "") 
                
                
                arquivo_de_saida.write(f"## {i+1}. `{caminho_do_arquivo}`\n\n")
                arquivo_de_saida.write(f"```{linguagem}\n")
                arquivo_de_saida.write(conteudo)
                arquivo_de_saida.write(f"\n```\n\n---\n\n")
                
                print(f"  [OK] Arquivo '{caminho_do_arquivo}' adicionado.")

        except FileNotFoundError:
            
            print(f"  [AVISO] Arquivo '{caminho_do_arquivo}' não encontrado. Pulando.")
        except Exception as e:
            print(f"  [ERRO] Ocorreu um erro ao ler o arquivo '{caminho_do_arquivo}': {e}")

print(f"\nAnálise concluída! O arquivo '{nome_do_arquivo_de_saida}' foi criado com sucesso na sua pasta.")
