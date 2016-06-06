# RedBear.Log4View.AzureServiceBus
A [Log4View](http://www.log4view.com/) plugin that reads log entries from a topic on an Azure Service Bus.

The benefits of using this receiver compared to using Log4View's built-in TCP or UDP receivers include:

* unlimited users can monitor the same log at the same time without you needing to touch the production application's configuration;
* there's no need to punch holes in firewalls or map external ports to internal ports; developers can choose which logs to stream without IT involvement.

## Getting Started
Either build the code yourself or install using [this installer](https://rbpublic.blob.core.windows.net/log4view/azure-plugin-setup.msi). The plugin must be installed into the "Log4View" folder in "Program Files (x86)" together with Microsoft.ServiceBus.dll.

A new receiver can be created by going to ```Settings > Edit Configuration``` within Log4View and then choosing "Azure Service Bus Receiver" from the ```Add``` button-menu.

Create log entries by using our [NLog target](https://github.com/RedBearSys/RedBear.Log4View.AzureServiceBus.Target) in the [usual way](https://github.com/nlog/nlog/wiki/Tutorial#writing-log-messages).

## Not seeing any log entries on your local machine?
Firstly, check whether you're seeing anything in your local log output from the plugin itself. If ithas run into any problems, it will output the details to your log using a logger name of "AzureServiceBus". **Note:** You may need to alter your Log4View filters to see this.

One of the most common issues is that plugin can't reach the Azure Service Bus because of firewall restrictions or anti-virus / anti-malware software preventing it from accessing the Internet. This should get reported in Log4View if it's the case. Your firewall should allow workstations to access remote IP addresses within the 9350 to 9354 port range.

It could also be that your Topic or Connection String for the application is incorrect. Again, this should surface in your log.
