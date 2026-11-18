# Bass Villain

Bass Villain é um jogo de ritmo para Windows feito em C# com Windows Forms e .NET Framework 4.7.2. Acerte as notas em quatro pistas, mantenha o combo para aumentar sua pontuação e desbloqueie novas músicas e dificuldades.

## Baixar e jogar

1. Abra a página de [Releases](https://github.com/barros-ns/BassVillain/releases) e escolha a versão desejada.
2. Em **Assets**, baixe o ZIP jogável para Windows. Os arquivos **Source code (zip)** e **Source code (tar.gz)** contêm o código-fonte.
3. Extraia todo o ZIP e abra `BassVillainApp.exe` na pasta extraída. Mantenha `BassVillainApp.exe.config` e as pastas `Imagens`, `Mapas` e `MusicasMp3` junto do executável.
4. Se o Windows solicitar o runtime, instale uma versão compatível do [.NET Framework 4.x](https://learn.microsoft.com/dotnet/framework/install/on-windows), a partir da 4.7.2.

## Executar pelo Visual Studio

1. Baixe o [Visual Studio 2022 Community](https://visualstudio.microsoft.com/vs/community/) e, no **Visual Studio Installer**, selecione a carga de trabalho **Desenvolvimento para desktop com .NET**.
2. Em **Componentes individuais**, procure o **Targeting Pack do .NET Framework 4.7.2**. Se ele não estiver disponível no instalador, baixe o [.NET Framework 4.7.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net472). O Developer Pack fornece as referências de compilação. [Guia da Microsoft](https://learn.microsoft.com/dotnet/framework/install/guide-for-developers)
3. No [repositório do Bass Villain](https://github.com/barros-ns/BassVillain), escolha **Code → Download ZIP** e extraia o código-fonte, ou clone o repositório com Git:

   ```bash
   git clone https://github.com/barros-ns/BassVillain.git
   ```

4. Abra `BassVillainApp.sln` no Visual Studio. Use `BassVillainApp` como projeto de inicialização e pressione **F5** para depurar ou **Ctrl+F5** para executar.
5. Para gerar o executável, selecione **Release** e vá em **Compilação → Recompilar Solução**. A saída fica em `BassVillainApp/bin/Release`. O projeto copia os mapas JSON, as imagens e os MP3s para a saída durante a compilação.

## Guia das telas

| Tela | Como funciona |
| --- | --- |
| **Menu inicial** | **Iniciar** abre a seleção de músicas. **Configurações** abre as preferências do jogador. |
| **Seleção de músicas** | Escolha uma faixa liberada. A tela consulta o histórico local para exibir os recordes por música. Em um perfil novo, comece com **Sultans of Swing**. |
| **Seleção de dificuldade** | Escolha **Fácil**, **Médio** ou **Difícil**, de acordo com os desbloqueios. Os recordes aqui são da combinação de música e dificuldade. **Voltar** leva à seleção de músicas. |
| **Partida** | Pressione a tecla da pista quando a nota atingir a linha de acerto. As teclas padrão são **A**, **S**, **K** e **L**, da esquerda para a direita. O painel mostra pontuação, combo, acertos, erros e progresso da faixa. **Esc** pausa e permite retomar ou voltar ao menu inicial. |
| **Conclusão** | Mostra pontuação, percentual de acertos, maior combo, rank e histórico da fase. O botão de retorno leva ao menu inicial. |
| **Configurações** | Ajuste volume, teclas e sincronismo do áudio em milissegundos. Use **Restaurar padrão** para repor as preferências iniciais. **Modo debug** exibe os botões de teste **Vencer** e **Perder** durante a partida. |

### Pontuação e desbloqueios

Cada acerto vale **100 pontos em Perfect**, **70 em Great** ou **40 em Good**, antes do bônus. A sequência de acertos aumenta o multiplicador conforme o rank do combo. Um **Miss** reinicia a sequência. Na conclusão, o rank representa o maior combo alcançado na partida.

Ao atingir **70% de acertos** para fins de desbloqueio, vencer no Fácil libera o Médio daquela faixa e, quando houver uma próxima música, o Fácil dela. Vencer no Médio libera o Difícil da mesma faixa.

O histórico e as preferências ficam em `%APPDATA%\BassVillain`. Os desbloqueios são persistidos nas configurações do usuário do Windows.

## Créditos musicais

| Faixa | Artista |
| --- | --- |
| Sultans of Swing | Dire Straits |
| Dark Necessities | Red Hot Chili Peppers |
| Seven Nation Army | The White Stripes |

Os direitos das composições e das gravações pertencem aos respectivos titulares.

