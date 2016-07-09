# Scheduler

This is a mashup of NServicebus, Quartz and CrystalQuartz to create a self-servicing Scheduler service.
It can be used in any SOA environment that needs timed actions, as an internal tool to provide scheduling support.
It's not much of a library, more of a way to work. The code load for the scheduler component is minimal, mostly wireup with the DI framework (Windsor)
