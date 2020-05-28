# Transcript
Transcript, a Google Cloud backed transcription tool.

## What is it, exactly?

Command line tool to ***transcribe one or multiple audio files simultaneously*** using Google Speech API.
Audio files can be either local or fetched from Google Storage. Transcriptions are saved in separate text files with recognition confidence indicator.

 
### Prerequisites
If you don't already have a *Google Cloud Platform Project* and related *Google Service Account*, you need to create one and download created account key file to your computer. It's a simple five step process, really, and it's described [in Google documentation](https://cloud.google.com/docs/authentication/production#creating_a_service_account) in detail. Guide will walk you through whole process.

At the time of writing, using said file is only authentication method supported by Google Speech API.

But then you'll have a file laying around on your computer that contains your private key and such. And that's not fun at all!

And because it's not fun, you'll need to create a encrypted key out of the file you just downloaded. This is, again, a simple process and you only need to provide one command and you are ready to go! Source file will be deleted after encryption if you don't explicitly prevent it.

Encrypted key will be unencrypted for the duration of transcribing and then removed. I know this is not optimal as it can still be left behind in case of application crash or some other misfortune.... but I'd say it's still better that nothing!

## How to use

### Creating the encrypted key

If your key file is called `my_plain_key.json`, you import it into Transcipt using command

`Transcript.exe --key --password 12345678 --file my_plain_key.json`

where `--password` is something you need to remember or store in a safe place. It is needed when transcribing and it can't be recovered. If you happen to lose your password, you need to download a new key from Google and repeat above command.

### Transcribing audio files

You can provide source file(s) from local machine using `--file` or from your Google Storage using `--url` switch. You can provide one file at a time or use `*` wildcard to process multiple audio files simultaneously.

`Transcript.exe --password 12345678 --file my-audio*.wav`<br>
`Transcript.exe --password 12345678 --url gs://my-google-bucket/my-audio*.wav`

All transcriptions will be stored in loca `result` folder when finished. **Note that when using Google Storage**, you need to provide storage access rights to your Service Account. Storage access right can be granted when you first create your Service Account or added to it later on.

### Providing additional parameters

If needed, you can explicitly set `language code`, `encoding` and `sample rate` to be used. You can see all parameters available by running Transcript without any parameters or using

`Transcript.exe --help`

### Example

There's an example audio provided with the project, in project root under `TestAudio`, called `preamble10.wav`.
Here's how one would go about transcibing that.

`Transcript.exe --password 12345678 --file preamble10.wav`

Or in more detail

`Transcript.exe --password 12345678 --file preamble10.wav --language en --encoding linear`

Result of that command would be 

>[98.03%] We the people of the United States in order to form a more perfect union establish justice insure domestic tranquility provide for the common defense.







