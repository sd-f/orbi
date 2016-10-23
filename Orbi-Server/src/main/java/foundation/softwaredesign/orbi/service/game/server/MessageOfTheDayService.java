package foundation.softwaredesign.orbi.service.game.server;

import foundation.softwaredesign.orbi.model.game.server.MessageOfTheDay;
import foundation.softwaredesign.orbi.persistence.repo.game.MessageOfTheDayRepository;
import foundation.softwaredesign.orbi.rest.exception.VersionNotSupportedException;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import java.util.Date;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class MessageOfTheDayService {

    @Inject
    MessageOfTheDayRepository repo;

    public List<MessageOfTheDay> loadLatestMessage() {
        return repo.findAllValid(new Date());
    }
}
